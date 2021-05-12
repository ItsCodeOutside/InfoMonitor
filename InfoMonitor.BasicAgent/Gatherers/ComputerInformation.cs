using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Management;
using InfoMonitor.DataSchema;

namespace InfoMonitor.BasicAgent.Gatherers
{
    public static class ComputerInformation
    {
        public static ComputerProperties Get()
        {
            ManagementScope scope = new ManagementScope(@"\\.\root\cimv2");
            //CPU
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT CurrentClockSpeed,NumberOfLogicalProcessors FROM Win32_Processor"));
            ManagementObjectCollection results = searcher.Get();
            uint totalCores = 0;

            List<uint> clockSpeeds = new List<uint>();
            foreach (ManagementObject item in results)
            {
                totalCores += (uint)item["NumberOfLogicalProcessors"];
                clockSpeeds.Add((uint)item["CurrentClockSpeed"]);
            }

            //return new object[] { totalCores, Average(clockSpeeds) };
            return new ComputerProperties()
            {
                MachineName = Environment.MachineName,
                AverageCurrentClockSpeed = Average(clockSpeeds),
                TotalRAMbytes = RAMInformation.Get(),
                TotalLogicalCPUCores = totalCores
            };
        }


        static uint Average(List<uint> data)
        {
            uint total = 0;
            foreach (uint value in data)
            {
                total += value;
            }
            return (uint)total / (uint)data.Count;
        }
    }
}
