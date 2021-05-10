using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Management;
using InfoMonitor.DataSchema;

namespace InfoMonitor.BasicAgent.Gatherers
{
    public static class NICInformation
    {
        public static List<NICProperties> Get()
        {
            ManagementScope scope = new ManagementScope(@"\\.\root\cimv2");
            //Network
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT Description, IPAddress, IPSubnet FROM Win32_NetworkAdapterConfiguration"));
            ManagementObjectCollection results = searcher.Get();

            var nicList = new List<NICProperties>();
            foreach (ManagementObject item in results)
            {
                if (null != item["IPAddress"])
                {
                    var nicInfo = new NICProperties();
                    nicInfo.Description = (string)item["Description"];
                    nicInfo.IPs = new List<IPProperties>();
                    string[] ips = (string[])item["IPAddress"];
                    string[] subnets = (string[])item["IPSubnet"];
                    for (int index = 0; index < ips.Length; index++)
                    {
                        nicInfo.IPs.Add(new IPProperties()
                        {
                            IP = ips[index],
                            Subnet = subnets[index]
                            
                        }) ;
                    }
                    nicList.Add(nicInfo);
                }
            }

            return nicList;
        }
    }
}
