using Legacy.Snapln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapIn.Wrapper
{
    // Migration stragegy: Wrapper / Adapter Pattern
    // this class sits between the legacy snap-in and the new .NET 8.0 snap-in, providing a translation layer

    // why wrap instead of rewrite?
    // 1. the legacy snap-in may have complex logic and state management that would be time-consuming and error-prone to rewrite from scratch
    // 2. wrapping allows us to reuse the existing snap-in code while gradually migrating functionality to the new .NET 8.0 snap-in
    // 3. the adapter translates calls from the new snap-in to the legacy snap-in, allowing us to maintain compatibility while we migrate features incrementally
    // 4. The gui never touches the legacy code directly

    // migration flow
    // legacy com (old code) --> adapter/wrapper (transalte types)  --> modern winform UI

    public class SnapInAdapter
    {
        private readonly SystemManagerSnapIn _legacySnapIn;

        public SnapInAdapter()
        {
            _legacySnapIn = new SystemManagerSnapIn();
        }
        public string DisplayName => _legacySnapIn.GetDisplayName();
        public string Description => _legacySnapIn.GetDescription();
        public string Version => _legacySnapIn.GetVersion();

        public SnapInNode BuildNodeTree()
        {
            var root = new SnapInNode
            {
                Name = _legacySnapIn.GetDisplayName(),
                IconKey = "computer"
            };

            foreach(var nodeName in _legacySnapIn.GetNodeNames())
            {
                var child = new SnapInNode
                {
                    Name = nodeName,
                    IconKey = GetIconForNode(nodeName)
                };
                var rawItems = _legacySnapIn.GetNodeItems(nodeName);
                
            }
            return root;
        }

        private static string GetIconForNode(string nodeName)
        {
            return nodeName switch
            {
                "Services" => "service",
                "Event Log" => "eventlog",
                "Configuration" => "registry",
                _ => "folder"
            };
        }
    }
}
