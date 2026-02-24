using SnapIn.Wrapper;

namespace Modern.ManagementConsole;

// ============================================================
// MODERN MANAGEMENT CONSOLE — WinForms Replacement for MMC
// ============================================================
// This form replicates the classic MMC layout:
//   ┌──────────────────────────────────────────────────┐
//   │  Menu Bar  |  Toolbar                            │
//   ├────────────┬─────────────────────────────────────┤
//   │            │                                     │
//   │  TreeView  │  ListView (details)                 │
//   │  (scope)   │                                     │
//   │            ├─────────────────────────────────────┤
//   │            │  PropertyGrid (properties pane)     │
//   │            │                                     │
//   ├────────────┴─────────────────────────────────────┤
//   │  Status Bar                                      │
//   └──────────────────────────────────────────────────┘
//
// The GUI talks to the legacy snap-in through the SnapInAdapter
// (wrapper layer) — it never touches COM or legacy code directly.
// ============================================================

public class ConsoleForm : Form
{
    private readonly SnapInAdapter _adapter = new();
    private readonly SnapInNode _rootNode;

    // Layout controls
    private readonly MenuStrip _menuStrip;
    private readonly ToolStrip _toolStrip;
    private readonly StatusStrip _statusStrip;
    private readonly SplitContainer _mainSplit;
    private readonly SplitContainer _rightSplit;
    private readonly TreeView _treeView;
    private readonly ListView _listView;
    private readonly PropertyGrid _propertyGrid;
    private readonly ToolStripStatusLabel _statusLabel;
    private readonly ToolStripStatusLabel _countLabel;

    public ConsoleForm()
    {
        _rootNode = _adapter.BuildNodeTree();

        Text = $"{_adapter.DisplayName} — Modern Management Console (.NET 8)";
        Size = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(800, 500);

        // =================== MENU BAR ===================
        _menuStrip = BuildMenuStrip();

        // =================== TOOLBAR ===================
        _toolStrip = BuildToolStrip();

        // =================== STATUS BAR ===================
        _statusStrip = new StatusStrip();
        _statusLabel = new ToolStripStatusLabel("Ready")
        {
            Spring = true,
            TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        };
        _countLabel = new ToolStripStatusLabel("0 items");
        _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _countLabel });

        // =================== TREE VIEW (left pane) ===================
        _treeView = new TreeView
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10),
            ShowLines = true,
            ShowPlusMinus = true,
            ShowRootLines = true,
            HideSelection = false,
            ItemHeight = 24
        };
        _treeView.AfterSelect += TreeView_AfterSelect;

        // =================== LIST VIEW (right-top pane) ===================
        _listView = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
            Font = new Font("Segoe UI", 9.5f),
            MultiSelect = false
        };
        _listView.SelectedIndexChanged += ListView_SelectedIndexChanged;

        // =================== PROPERTY GRID (right-bottom pane) ===================
        _propertyGrid = new PropertyGrid
        {
            Dock = DockStyle.Fill,
            HelpVisible = true,
            ToolbarVisible = false,
            Font = new Font("Segoe UI", 9.5f)
        };

        // =================== SPLIT CONTAINERS ===================
        // Right pane: ListView (top) + PropertyGrid (bottom)
        _rightSplit = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterDistance = 320,
            Panel1MinSize = 150,
            Panel2MinSize = 100
        };
        _rightSplit.Panel1.Controls.Add(_listView);

        var propLabel = new Label
        {
            Text = "  Properties",
            Dock = DockStyle.Top,
            Height = 26,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 60, 120),
            BackColor = Color.FromArgb(235, 238, 245),
            TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        };
        _rightSplit.Panel2.Controls.Add(_propertyGrid);
        _rightSplit.Panel2.Controls.Add(propLabel);

        // Main: TreeView (left) + right pane (right)
        _mainSplit = new SplitContainer
        {
            Dock = DockStyle.Fill,
            SplitterDistance = 260,
            Panel1MinSize = 180
        };
        _mainSplit.Panel1.Controls.Add(_treeView);
        _mainSplit.Panel2.Controls.Add(_rightSplit);

        // =================== ASSEMBLE ===================
        Controls.Add(_mainSplit);
        Controls.Add(_toolStrip);
        Controls.Add(_menuStrip);
        Controls.Add(_statusStrip);

        PopulateTree();
    }

    // =================== BUILD MENU ===================
    private MenuStrip BuildMenuStrip()
    {
        var menu = new MenuStrip();

        var fileMenu = new ToolStripMenuItem("&File");
        fileMenu.DropDownItems.Add("&Refresh All", null, (s, e) => RefreshAll());
        fileMenu.DropDownItems.Add(new ToolStripSeparator());
        fileMenu.DropDownItems.Add("E&xit", null, (s, e) => Close());

        var viewMenu = new ToolStripMenuItem("&View");
        viewMenu.DropDownItems.Add("&Large Icons", null, (s, e) => _listView.View = View.LargeIcon);
        viewMenu.DropDownItems.Add("&Details", null, (s, e) => _listView.View = View.Details);
        viewMenu.DropDownItems.Add("&List", null, (s, e) => _listView.View = View.List);

        var helpMenu = new ToolStripMenuItem("&Help");
        helpMenu.DropDownItems.Add("&About", null, (s, e) =>
            MessageBox.Show(
                $"{_adapter.DisplayName}\n" +
                $"Version: {_adapter.Version}\n\n" +
                $"{_adapter.Description}\n\n" +
                "This is a modern .NET 8 WinForms replacement\n" +
                "for a traditional MMC snap-in.",
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information));

        menu.Items.AddRange(new ToolStripItem[] { fileMenu, viewMenu, helpMenu });
        return menu;
    }

    // =================== BUILD TOOLBAR ===================
    private ToolStrip BuildToolStrip()
    {
        var strip = new ToolStrip();

        var refreshBtn = new ToolStripButton("Refresh") { ToolTipText = "Refresh data from snap-in" };
        refreshBtn.Click += (s, e) => RefreshAll();

        var expandBtn = new ToolStripButton("Expand All") { ToolTipText = "Expand all nodes" };
        expandBtn.Click += (s, e) => _treeView.ExpandAll();

        var collapseBtn = new ToolStripButton("Collapse All") { ToolTipText = "Collapse all nodes" };
        collapseBtn.Click += (s, e) => _treeView.CollapseAll();

        var infoLabel = new ToolStripLabel($"  |  {_adapter.DisplayName} v{_adapter.Version}")
        {
            ForeColor = Color.Gray
        };

        strip.Items.AddRange(new ToolStripItem[]
        {
            refreshBtn,
            new ToolStripSeparator(),
            expandBtn,
            collapseBtn,
            infoLabel
        });

        return strip;
    }

    // =================== TREE POPULATION ===================
    private void PopulateTree()
    {
        _treeView.Nodes.Clear();

        var rootTreeNode = new TreeNode(_rootNode.Name)
        {
            Tag = _rootNode
        };

        foreach (var child in _rootNode.Children)
        {
            var childNode = new TreeNode($"{child.Name}  ({child.Items.Count})")
            {
                Tag = child
            };
            rootTreeNode.Nodes.Add(childNode);
        }

        _treeView.Nodes.Add(rootTreeNode);
        _treeView.ExpandAll();

        _statusLabel.Text = $"Loaded {_rootNode.Children.Count} snap-in nodes from legacy component";
    }

    // =================== EVENT HANDLERS ===================
    private void TreeView_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not SnapInNode snapInNode)
            return;

        PopulateListView(snapInNode);
        _propertyGrid.SelectedObject = null;
        _statusLabel.Text = $"Selected: {snapInNode.Name}";
    }

    private void PopulateListView(SnapInNode node)
    {
        _listView.Items.Clear();
        _listView.Columns.Clear();

        if (node.Items.Count == 0 && node.Children.Count > 0)
        {
            // Root node — show children summary
            _listView.Columns.Add("Node", 200);
            _listView.Columns.Add("Items", 80);
            _listView.Columns.Add("Type", 120);

            foreach (var child in node.Children)
            {
                var item = new ListViewItem(child.Name);
                item.SubItems.Add(child.Items.Count.ToString());
                item.SubItems.Add(child.IconKey);
                _listView.Items.Add(item);
            }

            _countLabel.Text = $"{node.Children.Count} nodes";
            return;
        }

        // Determine columns from first item's properties
        if (node.Items.Count > 0)
        {
            var firstItem = node.Items[0];
            foreach (var key in firstItem.Properties.Keys)
            {
                int width = key == "Message" || key == "Value Data" || key == "Key Path" ? 280 : 130;
                _listView.Columns.Add(key, width);
            }

            foreach (var snapItem in node.Items)
            {
                var lvi = new ListViewItem(snapItem.Properties.Values.FirstOrDefault() ?? "");
                foreach (var val in snapItem.Properties.Values.Skip(1))
                {
                    lvi.SubItems.Add(val);
                }
                lvi.Tag = snapItem;

                // Color-code by status/level
                ColorCodeItem(lvi, snapItem);
                _listView.Items.Add(lvi);
            }
        }

        _countLabel.Text = $"{node.Items.Count} items";
    }

    private static void ColorCodeItem(ListViewItem lvi, SnapInListItem snapItem)
    {
        if (snapItem.Properties.TryGetValue("Status", out var status))
        {
            lvi.ForeColor = status switch
            {
                "Running" => Color.FromArgb(20, 120, 60),
                "Stopped" => Color.FromArgb(180, 40, 40),
                _ => Color.Black
            };
        }

        if (snapItem.Properties.TryGetValue("Level", out var level))
        {
            lvi.ForeColor = level switch
            {
                "Error" => Color.FromArgb(180, 40, 40),
                "Warning" => Color.FromArgb(180, 140, 0),
                _ => Color.Black
            };
        }
    }

    private void ListView_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_listView.SelectedItems.Count == 0)
        {
            _propertyGrid.SelectedObject = null;
            return;
        }

        var selected = _listView.SelectedItems[0].Tag as SnapInListItem;
        if (selected != null)
        {
            // Display properties in the PropertyGrid using a custom descriptor
            _propertyGrid.SelectedObject = new DictionaryPropertyGridAdapter(selected.Properties);
            _statusLabel.Text = $"Properties: {selected.Name} ({selected.Type})";
        }
    }

    private void InitializeComponent()
    {

    }

    private void RefreshAll()
    {
        _statusLabel.Text = "Refreshing data from legacy snap-in...";
        Cursor = Cursors.WaitCursor;

        try
        {
            var newAdapter = new SnapInAdapter();
            var newRoot = newAdapter.BuildNodeTree();

            _rootNode.Children.Clear();
            _rootNode.Children.AddRange(newRoot.Children);

            PopulateTree();
            _statusLabel.Text = "Refresh complete.";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error refreshing: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}

// ============================================================
// Helper: Makes a Dictionary<string,string> displayable in a PropertyGrid
// ============================================================
public class DictionaryPropertyGridAdapter : System.ComponentModel.ICustomTypeDescriptor
{
    private readonly Dictionary<string, string> _properties;

    public DictionaryPropertyGridAdapter(Dictionary<string, string> properties)
    {
        _properties = properties;
    }

    public System.ComponentModel.AttributeCollection GetAttributes() =>
        System.ComponentModel.TypeDescriptor.GetAttributes(this, true);

    public string? GetClassName() => null;
    public string? GetComponentName() => null;

    public System.ComponentModel.TypeConverter GetConverter() =>
        System.ComponentModel.TypeDescriptor.GetConverter(this, true);

    public System.ComponentModel.EventDescriptor? GetDefaultEvent() => null;
    public System.ComponentModel.PropertyDescriptor? GetDefaultProperty() => null;

    public object? GetEditor(Type editorBaseType) =>
        System.ComponentModel.TypeDescriptor.GetEditor(this, editorBaseType, true);

    public System.ComponentModel.EventDescriptorCollection GetEvents() =>
        System.ComponentModel.EventDescriptorCollection.Empty;

    public System.ComponentModel.EventDescriptorCollection GetEvents(Attribute[]? attributes) =>
        System.ComponentModel.EventDescriptorCollection.Empty;

    public System.ComponentModel.PropertyDescriptorCollection GetProperties() => GetProperties(null);

    public System.ComponentModel.PropertyDescriptorCollection GetProperties(Attribute[]? attributes)
    {
        var descriptors = _properties.Keys
            .Select(key => new DictionaryPropertyDescriptor(key, _properties))
            .Cast<System.ComponentModel.PropertyDescriptor>()
            .ToArray();

        return new System.ComponentModel.PropertyDescriptorCollection(descriptors);
    }

    public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor? pd) => this;
}

internal class DictionaryPropertyDescriptor : System.ComponentModel.PropertyDescriptor
{
    private readonly string _key;
    private readonly Dictionary<string, string> _dict;

    public DictionaryPropertyDescriptor(string key, Dictionary<string, string> dict)
        : base(key, null)
    {
        _key = key;
        _dict = dict;
    }

    public override Type ComponentType => typeof(DictionaryPropertyGridAdapter);
    public override bool IsReadOnly => true;
    public override Type PropertyType => typeof(string);
    public override bool CanResetValue(object component) => false;
    public override object? GetValue(object? component) => _dict.TryGetValue(_key, out var val) ? val : null;
    public override void ResetValue(object component) { }
    public override void SetValue(object? component, object? value) { }
    public override bool ShouldSerializeValue(object component) => false;
}
