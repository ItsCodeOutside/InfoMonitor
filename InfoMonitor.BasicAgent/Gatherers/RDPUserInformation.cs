using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using InfoMonitor.DataSchema;

namespace InfoMonitor.BasicAgent.Gatherers
{
    public static class RDPUserInformation
    {
        public static List<RDPUserProperties> Get()
        {
            var result = new List<RDPUserProperties>();

            ManagementScope scope = new ManagementScope(@"\\.\root\cimv2");
            ObjectQuery query = new ObjectQuery("SELECT LogonId, Name, Status FROM Win32_LogonSession Where LogonType = 10");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection results = searcher.Get();
            foreach (ManagementObject item in results)
            {

                result.Add(new RDPUserProperties() { 
                    SessionId = (int)item["LogonId"],
                    Username = item["Name"].ToString() ,
                    Status = item["Status"].ToString()
                });
            }

            return result;
        }
    }
}
