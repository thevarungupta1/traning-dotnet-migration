namespace Legacy.SnapIn.Models;

/// <summary>
/// Represents a Windows service as seen by the snap-in.
/// </summary>
public class ManagedService
{
    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Status { get; set; } = "Unknown";
    public string StartupType { get; set; } = "Automatic";
    public string Account { get; set; } = "LocalSystem";
    public string Description { get; set; } = "";
}
