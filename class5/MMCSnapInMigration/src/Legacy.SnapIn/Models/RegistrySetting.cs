namespace Legacy.SnapIn.Models;

/// <summary>
/// Represents a registry-based configuration setting.
/// </summary>
public class RegistrySetting
{
    public string KeyPath { get; set; } = "";
    public string ValueName { get; set; } = "";
    public string ValueData { get; set; } = "";
    public string ValueType { get; set; } = "REG_SZ";
}
