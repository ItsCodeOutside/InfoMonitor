using System.Collections.Generic;

namespace InfoMonitor.DataSchema
{
    public class NICProperties
    {
        public string Description { get; set; }
        public List<IPProperties> IPs { get; set; }
    }
}
