using Legacy.SnapIn;
using Legacy.SnapIn.Models;

namespace SnapIn.Wrapper;

// ============================================================
// MIGRATION STRATEGY: Wrapper / Adapter Pattern
// ============================================================
// This class sits BETWEEN the legacy COM snap-in and the modern GUI.
//
// Why wrap instead of rewrite?
//   1. The legacy snap-in may have complex logic built over years
//   2. Wrapping is lower risk — existing logic stays untouched
//   3. The adapter translates COM-style untyped data into
//      modern strongly-typed classes
//   4. The GUI never touches the legacy code directly
//
// Migration flow:
//   [Legacy COM Snap-In] → [Adapter/Wrapper] → [Modern WinForms GUI]
//          ↑ old code            ↑ bridge            ↑ new code
//          unchanged        translates types     clean modern UI
// ============================================================

public class SnapInAdapter
{
    private readonly SystemManagerSnapIn _legacySnapIn;

    public SnapInAdapter()
    {
        // In a real migration, this might use COM interop to create
        // the legacy object via its CLSID:
        //   Type comType = Type.GetTypeFromCLSID(new Guid("F1E2D3C4-..."));
        //   var instance = Activator.CreateInstance(comType) as ISnapInComponent;
        //
        // For this demo, we instantiate directly since both projects
        // are in the same solution.
        _legacySnapIn = new SystemManagerSnapIn();
    }

    public string DisplayName => _legacySnapIn.GetDisplayName();
    public string Description => _legacySnapIn.GetDescription();
    public string Version => _legacySnapIn.GetVersion();

    /// <summary>
    /// Builds a modern tree structure from the legacy snap-in's flat data.
    /// The legacy interface only returns string[] node names and object[] items.
    /// This adapter converts them into a proper tree with typed list items.
    /// </summary>
    public SnapInNode BuildNodeTree()
    {
        var root = new SnapInNode
        {
            Name = _legacySnapIn.GetDisplayName(),
            IconKey = "computer"
        };

        foreach (var nodeName in _legacySnapIn.GetNodeNames())
        {
            var child = new SnapInNode
            {
                Name = nodeName,
                IconKey = GetIconForNode(nodeName)
            };

            var rawItems = _legacySnapIn.GetNodeItems(nodeName);
            child.Items = ConvertToListItems(nodeName, rawItems);
            root.Children.Add(child);
        }

        return root;
    }

    /// <summary>
    /// Gets typed service data from the legacy snap-in.
    /// </summary>
    public List<ManagedService> GetServices() => _legacySnapIn.GetServices();

    /// <summary>
    /// Gets typed event log data from the legacy snap-in.
    /// </summary>
    public List<EventLogEntry> GetEventLogEntries() => _legacySnapIn.GetEventLogEntries();

    /// <summary>
    /// Gets typed registry settings from the legacy snap-in.
    /// </summary>
    public List<RegistrySetting> GetRegistrySettings() => _legacySnapIn.GetRegistrySettings();

    // Converts the legacy untyped object[] into modern SnapInListItem list
    private static List<SnapInListItem> ConvertToListItems(string nodeName, object[] rawItems)
    {
        var items = new List<SnapInListItem>();

        foreach (var raw in rawItems)
        {
            var item = raw switch
            {
                ManagedService svc => new SnapInListItem
                {
                    Name = svc.DisplayName,
                    Type = "Service",
                    Properties = new Dictionary<string, string>
                    {
                        ["Service Name"] = svc.Name,
                        ["Display Name"] = svc.DisplayName,
                        ["Status"] = svc.Status,
                        ["Startup Type"] = svc.StartupType,
                        ["Log On As"] = svc.Account,
                        ["Description"] = svc.Description
                    }
                },
                EventLogEntry evt => new SnapInListItem
                {
                    Name = $"Event {evt.EventId}",
                    Type = "Event",
                    Properties = new Dictionary<string, string>
                    {
                        ["Event ID"] = evt.EventId.ToString(),
                        ["Source"] = evt.Source,
                        ["Level"] = evt.Level,
                        ["Time"] = evt.TimeGenerated.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["Category"] = evt.Category,
                        ["Message"] = evt.Message
                    }
                },
                RegistrySetting reg => new SnapInListItem
                {
                    Name = reg.ValueName,
                    Type = "Registry",
                    Properties = new Dictionary<string, string>
                    {
                        ["Key Path"] = reg.KeyPath,
                        ["Value Name"] = reg.ValueName,
                        ["Value Data"] = reg.ValueData,
                        ["Value Type"] = reg.ValueType
                    }
                },
                _ => new SnapInListItem
                {
                    Name = raw.ToString() ?? "Unknown",
                    Type = "Unknown"
                }
            };

            items.Add(item);
        }

        return items;
    }

    private static string GetIconForNode(string nodeName) => nodeName switch
    {
        "Services" => "services",
        "Event Log" => "eventlog",
        "Configuration" => "config",
        _ => "folder"
    };
}
