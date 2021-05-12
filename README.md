# InfoMonitor
This project provides an extremely simple dashboard for technical people to display and manipulate data from multiple sources. The concept is simple, the web site will accept JSON and cache it in memory then reply with that data. The front-end then allows you to use javscript to display it however you like.

This is written to be as simple to read as possible so that non-programmers can still use and modify the dashboard (IE:/ support technicians, network engineers, managers - anyone technical but not codey)

## Dependencies
* Web project
  * .Net Core 3.1
  * [jquery](https://github.com/jquery/jquery) 
  * [linq.js](https://github.com/mihaifm/linq)
  * [Bootstrap](https://github.com/twbs/bootstrap/)
* Console application (Example of agent)
  * Microsoft.Extensions.Configuration.Json
  * System.Management

## Getting Started
1. Download reopsitory
2. Open in Visual Studio
3. Build & restore NuGet Packages
4. Run
5. See that the console application says it has sent an update
6. Click the HTML button labelled 'Refresh'

The included BasicAgent uses Windows Management Instrumentation to gather machine name, CPU cores & clock speed, RAM amount, and IP addresses. You can use linq.js methods to display the data in any form you need.

## Going Further
The data gathering agent can be in any language, on any platform, and return any type of data. This can be used to build a dashboard on near-current activity to provide an overview of your estate and allows you to quickly drill down on any set of data.

I found [wmie2](https://github.com/vinaypamnani/wmie2) to be useful when looking at Windows Management Instrumentation for certain information.