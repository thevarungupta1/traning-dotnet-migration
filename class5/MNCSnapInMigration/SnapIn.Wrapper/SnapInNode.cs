using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapIn.Wrapper
{
    public class SnapInNode
    {
        public string Name { get; set; } = "";
        public string IconKey { get; set; } = "folder";
        public List<SnapInNode> Children { get; set; } = new();
        public List<SnapInListItem> Items { get; set; } = new();
    }

    public class SnapInListItem
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public Dictionary<string, string> Properties { get; set; } = new();
    }
}
