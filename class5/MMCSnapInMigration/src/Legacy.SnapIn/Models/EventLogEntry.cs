namespace Legacy.SnapIn.Models;

/// <summary>
/// Represents an event log entry as seen by the snap-in.
/// </summary>
public class EventLogEntry
{
    public int EventId { get; set; }
    public string Source { get; set; } = "";
    public string Level { get; set; } = "Information";
    public string Message { get; set; } = "";
    public DateTime TimeGenerated { get; set; } = DateTime.Now;
    public string Category { get; set; } = "None";
}
