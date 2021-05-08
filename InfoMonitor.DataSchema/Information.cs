using System;
using System.Collections.Generic;

namespace InfoMonitor.DataSchema
{
    public class Information
    {
        public ComputerProperties Computer { get; set; }
        public List<ProcessProperties> Processes { get; set; }
        public List<RDPUser> RDPUsers { get; set; }
    }
}
