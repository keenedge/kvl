using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Serilog;
using Microsoft.AspNetCore.Hosting;

namespace Zone.Services
{

    /// <summary/>
    public sealed class ThinkLogicalManagerService : IThinkLogicalManagerService
    {

        readonly Serilog.ILogger _log = Log.ForContext<ThinkLogicalManagerService>();
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary/>
        public Dictionary<string, Models.ThinkLogicalDevice> ThinkLogicalDevices = new Dictionary<string, Models.ThinkLogicalDevice>()
        {
            { "vx320", new Models.ThinkLogicalDevice() { Name = "vx320", HostName = "tl-vx320.control.vis.kaust.edu.sa", IpAddress = "192.168.5.15" } },
            { "tlx24", new Models.ThinkLogicalDevice() { Name = "tlx24", HostName = "tlx24-2416.control.vis.kaust.edu.sa", IpAddress = "192.168.5.81" } }
        };

        /// <summary/>
        public ThinkLogicalManagerService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary/>
        public void ApplyRoutes(string deviceName, IEnumerable<string> commands, List<string> results)
        {
            if (commands == null)
            {
                return;
            }

            IEnumerable<string> routes = ExpandCommands(commands);

            if (routes.Count() <= 0)
            {
                _log.Information("Expanded routes list is empty");
                return;
            }
            _log.Debug(String.Join(',', routes));

            string message = "";
            message = $"ThinkLogicalDevices";
            _log.Warning(message);
            results.Add(message);

            foreach (String device in ThinkLogicalDevices.Keys)
            {
                message = $"Device: {device}";
                _log.Warning(message);
                results.Add(message);
            }

            if (!_hostingEnvironment.IsProduction())
            {
                message = $"Routes cannot be applied in a non production environment. [{_hostingEnvironment.EnvironmentName}]";
                _log.Warning(message);
                results.Add(message);
            }

            if (!ThinkLogicalDevices.ContainsKey(deviceName))
            {
                message = $"{deviceName} is not a valid think logical device name";
                _log.Warning(message);
                results.Add(message);
            }
            else
            {
                string hostName = ThinkLogicalDevices[deviceName].HostName;
                
                if (_hostingEnvironment.IsProduction())
                {
                    TcpClient device = new TcpClient(hostName, 17567);
                    NetworkStream deviceStream = device.GetStream();

                    try
                    {
                        foreach (String route in routes)
                        {
                            _log.Debug(route);

                            byte[] data = Encoding.ASCII.GetBytes(route + "\n");
                            byte[] buffer = new byte[1024];

                            deviceStream.Write(data, 0, data.Length);
                            deviceStream.Flush();

                            int numBytesRead = deviceStream.Read(buffer, 0, 1024);

                            String response = Encoding.ASCII.GetString(buffer, 0, numBytesRead);

                            _log.Debug(response);
                        }
                    }
                    finally
                    {
                        deviceStream.Close();
                    }
                }
            }
        }

        /// <summary/>
        private IEnumerable<string> ExpandCommands(IEnumerable<String> commands)
        {
            List<String> routes = new List<string>();
            foreach (var command in commands)
            {
                IEnumerable<string> newRoutes = ExpandCommand(command);
                routes.AddRange(newRoutes);
            }
            return routes;
        }

        // converts the short hand format for a list of think logical routes
        // IN: Short hand commandTemplate. E.g.
        //      AIiiiiOoooo:nn
        //        A: Action [ D=Disconnect, C=Connect]
        //        I: Input Chassis [ I=Upper, i=Lower]
        //        O: Output Chassis [ O=Upper, o=Lower]
        //        iiii: first input port number
        //        oooo: first outputt port number
        //        nn: number of times to repeat while expnadning the command template to a list of commands
        //      Ci0100o0200:10
        private IEnumerable<string> ExpandCommand(string commandTemplate)
        {
            List<string> commands = new List<String>();

            string pattern = "(?<action>(D|C|c|d))(?<chassis1>(i|I|o|O))(?<port1>\\d{4})((?<chassis2>(i|I|o|O))(?<port2>\\d{4}))?(:(?<repeat>\\d{1,3}))?";
            MatchCollection matches = Regex.Matches(commandTemplate, pattern);

            if (matches.Count > 0)
            {
                Match m = matches[0];

                int repeat = 1;
                int.TryParse(m.Groups["repeat"].Value, out repeat);
                if (repeat == 0)
                    repeat = 1;
                string action = m.Groups["action"].Value;
                string chassis1 = m.Groups["chassis1"].Value;
                string chassis2 = m.Groups["chassis2"].Value;
                int port1 = 0;
                int.TryParse(m.Groups["port1"].Value, out port1);
                int port2 = 0;
                int.TryParse(m.Groups["port2"].Value, out port2);

                switch (action)
                {
                    case "D":
                    case "d":
                        for (int i = 0; i < repeat; i++)
                        {
                            string command = String.Format("{0}{1}{2,4:D4}", action, chassis1, port1++);
                            commands.Add(command);
                        }
                        break;


                    case "C":
                    case "c":
                        for (int i = 0; i < repeat; i++)
                        {
                            string command = String.Format("{0}{1}{2,4:D4}{3}{4,4:D4}", action, chassis1, port1++, chassis2, port2++);
                            commands.Add(command);
                        }

                        break;
                }
            }

            return commands;
        }
    }

}