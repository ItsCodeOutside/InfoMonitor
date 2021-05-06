using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoMonitor.Web
{
    public class SourceItem
    {
        public string SourceId { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}
