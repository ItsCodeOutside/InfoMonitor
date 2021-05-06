using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Management;

namespace InfoMonitor.BasicAgent.Gatherers
{
    public static class NICInformation
    {
        public static object Get()
        {
            ManagementScope scope = new ManagementScope(@"\\.\root\cimv2");
            //Network
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT IPAddress FROM Win32_NetworkAdapterConfiguration"));
            ManagementObjectCollection results = searcher.Get();

            List<string> nicList = new List<string>();
            foreach (ManagementObject item in results)
            {
                if (null != item["IPAddress"])
                {
                    string[] ips = (string[])item["IPAddress"];
                    for (int index = 0; index < ips.Length; index++)
                    {
                        nicList.AddRange(ips);
                    }
                }
            }

            return nicList;
        }
    }
}
