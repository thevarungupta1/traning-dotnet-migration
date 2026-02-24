namespace SnapIn.Wrapper;

/// <summary>
/// A modern, strongly-typed representation of a snap-in tree node.
/// Replaces the raw COM objects and untyped arrays from the legacy interface.
/// </summary>
public class SnapInNode
{
    public string Name { get; set; } = "";
    public string IconKey { get; set; } = "folder";
    public List<SnapInNode> Children { get; set; } = new();
    public List<SnapInListItem> Items { get; set; } = new();
}

/// <summary>
/// A modern representation of a list item (right-hand pane in MMC).
/// Replaces the untyped object[] from the legacy COM interface.
/// </summary>
public class SnapInListItem
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public Dictionary<string, string> Properties { get; set; } = new();
}
