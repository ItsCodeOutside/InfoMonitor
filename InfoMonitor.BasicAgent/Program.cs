using System;
using System.Threading;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.IO;
using InfoMonitor.BasicAgent.Gatherers;

namespace InfoMonitor.BasicAgent
{
    class Program
    {
        static bool _running = true;
        static string _infoMonitorUrl;
        static int _updateDelayMs = 5000;
        static void Main(string[] args)
        {
            //appsettings.json properties must be set to copy to output directory
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var appSettings = config.GetSection("AppSettings");
            _infoMonitorUrl = appSettings["InfoMonitorUrl"];
            string updateDelaySeconds = appSettings["UpdateDelaySeconds"];
            if (!int.TryParse(updateDelaySeconds, out _updateDelayMs))
            {
                _updateDelayMs = 5000;
            }

            //Do the actual gathering and reporting in a background thread
            Thread reportingThread = new Thread(new ThreadStart(LoopIt));
            reportingThread.Start();
            Console.WriteLine("This example agent is intended for use on Windows");
            Console.WriteLine("Press enter to finish the program");
            Console.ReadLine();
            Console.WriteLine("Exiting...");

            //Lets the loop end
            _running = false;
        }

        static void LoopIt()
        {
            Console.WriteLine($"Reporting data to {_infoMonitorUrl}");
            while (_running)
            {
                //GET DATA HERE. This is just an example and should be replaced with a better structured object
                var data = new object[] {
                    "MachineName", $"{Environment.MachineName}",
                    "CPUInformation", CPUInformation.Get(),
                    "RAMInformation", RAMInformation.Get(),
                    "IPAddresses", NICInformation.Get()
                };

                string json = JsonSerializer.Serialize(data);
                var sender = WebRequest.Create(_infoMonitorUrl);
                sender.Method = "POST";
                sender.ContentType = "application/json";
                sender.ContentLength = json.Length;
                using (var sw = new StreamWriter(sender.GetRequestStream()))
                {
                    sw.Write(json);
                    sw.Close();
                }
                try
                {
                    var response = (HttpWebResponse)sender.GetResponse();
                    Console.WriteLine($"{DateTime.Now}\tUpdate sent!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now}\t{ex.GetType().FullName}: {ex.Message}");
                }
                Thread.Sleep(_updateDelayMs);
            }
        }
    }
}
