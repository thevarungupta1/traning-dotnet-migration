using Legacy.Snapln.Models;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.ServiceProcess;


namespace Legacy.Snapln
{
    // Legacy SNAP-In Implementation (Com Component)
    // this simulates a real MMC snap-in that was registerd as a com
    // component. Tradiotnal snap-in
    // 1. were decorated with COm attributes
    // 2. registerd themeselve in the window registry
    // 3. used serviceController, eventLog, Registry API
    // 4. Were loaded by mmc.exe via COM activation


    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class SystemManagerSnapIn : ISnapInComponent
    {
        private readonly string _machineName;

        public SystemManagerSnapIn()
        {
            _machineName = Environment.MachineName;
        }

        public string GetDisplayName() => "System Manager";

        public string GetDescription() =>
            $"Manages services, events, and configuration on {_machineName}";

        public string GetVersion() => "1.0.0 (Legacy)";

        public string[] GetNodeNames() =>
            new[] { "Services", "Event Log", "Configuration" };

        public object[] GetNodeItems(string nodeName)
        {
            return nodeName switch
            {
                "Services" => GetServices().Cast<object>().ToArray(),
                "Event Log" => GetEventLogEntries().Cast<object>().ToArray(),
                "Configuration" => GetRegistrySettings().Cast<object>().ToArray(),
                _ => Array.Empty<object>()
            };
        }

        public int GetItemCount(string nodeName)
        {
            return GetNodeItems(nodeName).Length;
        }


        public List<ManagedService> GetServices()
        {
            var services = new List<ManagedService>();

            try
            {
                foreach (var sc in ServiceController.GetServices().Take(25))
                {
                    services.Add(new ManagedService
                    {
                        Name = sc.ServiceName,
                        DisplayName = sc.DisplayName,
                        Status = sc.Status.ToString(),
                        StartupType = GetStartupType(sc.ServiceName),
                        Account = "LocalSystem",
                        Description = $"Windows Service: {sc.DisplayName}"
                    });
                }
            }
            catch (Exception ex)
            {
                services.Add(new ManagedService
                {
                    Name = "Error",
                    DisplayName = $"Could not read services: {ex.Message}",
                    Status = "Error"
                });
            }

            return services;
        }

        /// <summary>
        /// Reads real Windows Event Log entries.
        /// EventLog class requires Windows Compatibility Pack in .NET 8.
        /// </summary>
        public List<Models.EventLogEntry> GetEventLogEntries()
        {
            var entries = new List<Models.EventLogEntry>();

            try
            {
                using var log = new System.Diagnostics.EventLog("Application");
                foreach (System.Diagnostics.EventLogEntry entry in log.Entries.Cast<System.Diagnostics.EventLogEntry>().TakeLast(20))
                {
                    entries.Add(new Models.EventLogEntry
                    {
                        EventId = (int)entry.InstanceId,
                        Source = entry.Source,
                        Level = entry.EntryType.ToString(),
                        Message = entry.Message.Length > 200
                            ? entry.Message[..200] + "..."
                            : entry.Message,
                        TimeGenerated = entry.TimeGenerated,
                        Category = entry.Category
                    });
                }
            }
            catch (Exception ex)
            {
                entries.Add(new Models.EventLogEntry
                {
                    EventId = 0,
                    Source = "Error",
                    Message = $"Could not read event log: {ex.Message}"
                });
            }

            return entries;
        }

        /// <summary>
        /// Reads registry settings. Registry class requires
        /// Windows Compatibility Pack in .NET 8.
        /// </summary>
        public List<RegistrySetting> GetRegistrySettings()
        {
            var settings = new List<RegistrySetting>();

            try
            {
                string[] keyPaths =
                {
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                @"SOFTWARE\Microsoft\.NETFramework"
            };

                foreach (var keyPath in keyPaths)
                {
                    using var key = Registry.LocalMachine.OpenSubKey(keyPath);
                    if (key == null) continue;

                    foreach (var valueName in key.GetValueNames().Take(10))
                    {
                        var value = key.GetValue(valueName);
                        settings.Add(new RegistrySetting
                        {
                            KeyPath = $@"HKLM\{keyPath}",
                            ValueName = valueName,
                            ValueData = value?.ToString() ?? "(null)",
                            ValueType = key.GetValueKind(valueName).ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                settings.Add(new RegistrySetting
                {
                    KeyPath = "Error",
                    ValueName = "Error",
                    ValueData = ex.Message
                });
            }

            return settings;
        }

        private static string GetStartupType(string serviceName)
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(
                    $@"SYSTEM\CurrentControlSet\Services\{serviceName}");
                var startValue = key?.GetValue("Start");
                return startValue switch
                {
                    0 => "Boot",
                    1 => "System",
                    2 => "Automatic",
                    3 => "Manual",
                    4 => "Disabled",
                    _ => "Unknown"
                };
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}

