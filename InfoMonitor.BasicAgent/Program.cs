using System;
using System.Threading;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.IO;
using InfoMonitor.BasicAgent.Gatherers;
using InfoMonitor.DataSchema;

namespace InfoMonitor.BasicAgent
{
    /*
    This is only intended to be a simple example of how useful data can be captured and sent to the server for reporting.
    */

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

            //Do the actual gathering and reporting in a separate thread.
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
                //GET DATA HERE. This is just an example, you can use your own object in your data collection agent
                var data = new Information()
                {
                    Computer = ComputerInformation.Get(),
                    Processes = ProcessInformation.Get(),
                    RDPUsers = RDPUserInformation.Get()
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
                    if (response.StatusCode == HttpStatusCode.OK)
                        Console.WriteLine($"{DateTime.Now}\tUpdate sent!");
                    else
                        Console.WriteLine($"{DateTime.Now}\tServer replied {response.StatusCode}");
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
