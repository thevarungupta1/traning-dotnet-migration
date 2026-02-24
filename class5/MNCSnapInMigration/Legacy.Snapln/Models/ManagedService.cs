using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legacy.Snapln.Models
{
    public class ManagedService
    {
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Status { get; set; } = "Unknown";
        public string StartupType { get; set; } = "Automatic";
        public string Account { get; set; } = "LocalSystem";
        public string Description { get; set; } = "";
    }
}
