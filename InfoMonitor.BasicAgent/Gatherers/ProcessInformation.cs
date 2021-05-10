using System;
using System.Collections.Generic;
using System.Text;
using InfoMonitor.DataSchema;
using System.Diagnostics;

namespace InfoMonitor.BasicAgent.Gatherers
{
    public static class ProcessInformation
    {
        public static List<ProcessProperties> Get()
        {
            var processes = Process.GetProcesses();
            List<ProcessProperties> results = new List<ProcessProperties>();
            foreach (var proc in processes)
            {
                results.Add(new ProcessProperties()
                {
                    ProcessId = proc.Id,
                    SessionId = proc.SessionId,
                    Name = proc.ProcessName,
                    Responding = proc.Responding,
                    RAMBytes = proc.WorkingSet64
                });
            }
            return results;
        }
    }
}
