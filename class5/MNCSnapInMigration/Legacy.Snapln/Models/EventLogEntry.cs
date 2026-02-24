using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legacy.Snapln.Models
{
    public class EventLogEntry
    {
        public int EventId { get; set; }
        public string Source { get; set; } = "Information";
        public string Level { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime TimeGenerated { get; set; } = DateTime.Now;
        public string Category { get; set; }=  "None";
    }
}
