using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Management;

namespace InfoMonitor.BasicAgent.Gatherers
{
    public static class RAMInformation
    {
        public static ulong Get()
        {
            ManagementScope scope = new ManagementScope(@"\\.\root\cimv2");
            //Memory
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory"));
            ManagementObjectCollection results = searcher.Get();
            ulong memoryCapacity = 0;

            foreach (ManagementObject item in results)
            {
                memoryCapacity += (ulong)item["Capacity"];
            }

            return memoryCapacity;
        }
    }
}
