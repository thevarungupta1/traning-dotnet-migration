namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Feature 1 : ICommand binding on Button.Command (.Net 8)
        // Instead of btnLogin.Click += handler, we assign an ICommand.
        // The button automatically calls CanExecute to enable/diable itself
        // and calls Execute when clicked - exact;y like WPF
        private readonly LoginViewModel _viewModel = new();

        public Form1()
        {
            InitializeComponent();

            SetupCommandBindings();
            SetupDataBindings();
            LoadSystemIcons();
        }

        private void SetupCommandBindings()
        {
            button1.Command = _viewModel.LoginCommand;
            button2.Command = _viewModel.ClearCommand;
        }

        private void SetupDataBindings()
        {
            textBox1.DataBindings.Add(
                nameof(TextBox.Text), _viewModel, nameof(_viewModel.Username),
                false, DataSourceUpdateMode.OnPropertyChanged);
            textBox2.DataBindings.Add(
                nameof(TextBox.Text), _viewModel, nameof(_viewModel.Password),
                false, DataSourceUpdateMode.OnPropertyChanged);

            label4.DataBindings.Add(
                nameof(Label.Text), _viewModel, nameof(_viewModel.StatusMessage),
                false, DataSourceUpdateMode.OnPropertyChanged);
        }

        // Feature 2: SystemIcons.GetStockIcon (.Net 8)
        // New Api that returns high-quality, DPI-aware system icons by
        // StockIconId - no more hardcoding icon indexes or using old Win32 APIs

        private void LoadSystemIcons()
        {
            var iconsToShow = new (StockIconId id, string Label)[]
            {
                (StockIconId.Application, "Application"),
                (StockIconId.Warning, "Warning"),
                (StockIconId.Error, "Error"),
                (StockIconId.Info, "Information"),
                (StockIconId.Shield, "Shield"),
                (StockIconId.Folder, "Folder"),
                (StockIconId.FolderOpen, "Folder Open"),
                (StockIconId.Rename, "Rename"),
                (StockIconId.Delete, "Delete"),
                (StockIconId.DesktopPC, "Desktop PC")

            };

            foreach (var (id, label) in iconsToShow)
            {
                Icon stockIcon = SystemIcons.GetStockIcon(id, StockIconOptions.SmallIcon);
                var row = new Panel { Size = new Size(300, 40) };

                var pic = new PictureBox
                {
                    Image = stockIcon.ToBitmap(),
                    Location = new Point(0, 0),
                    Size = new Size(32, 32),
                    SizeMode = PictureBoxSizeMode.CenterImage,
                };

                var lbl = new Label
                {
                    Text = $"{label} (StockIconId.{id})",
                    Location = new Point(40, 0),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10)
                };

                row.Controls.Add(pic);
                row.Controls.Add(lbl);
                flowIcons.Controls.Add(row);
            }
        }
    }
}
