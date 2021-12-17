using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using P2P.Utility;
using System.Net.Http;

namespace P2P.Terminal
{
    public class TerminalHandler
    {
        public static void Run(IConfigLoader configs, HttpClient httpClient)
        {
            if(configs.Config.FriendNodes == null)
            {
                Console.WriteLine("No friend nodes, cannot request anything");
                return;
            }

            while(true)
            { 
                var input = Console.ReadLine();
                var parts = input.Split(' ');
                if(parts[0] != "request")
                {
                    Console.WriteLine("WRONG FORMAT, must be: request filename");
                    continue;
                }

                if(parts.Length == 1 || parts[1].Trim() == "")
                {
                    Console.WriteLine("Invalid filename, format must be: request filename");
                    continue;
                }

                if(configs.Config.OwnedFiles.Contains(parts[1]))
                {
                    Console.WriteLine("File is already available on this Node.");
                    continue;
                }

                var validNodes = new List<int>();
                foreach(var n in configs.NodeFilesInfo.NodeFiles)
                {
                    foreach(var f in n.NodeFiles)
                    {
                        if(f == parts[1])
                        {
                            validNodes.Add(n.NodeName);
                            break;
                        }
                    }
                }

                if (validNodes.Count == 0)
                {
                    Console.WriteLine("No node has this file!");
                    continue;
                }

                int address = -1;
                int targetName = -1;
                foreach(var n in validNodes)
                {
                    var result = httpClient.FindAddress(n, configs.Config).Result;
                    if(result != -1)
                    {
                        address = result;
                        targetName = n;
                        break;
                    }
                }
                if(address == -1)
                {
                    Console.WriteLine("Impossible to find address");
                    continue;
                }

                httpClient.DownloadFile(targetName, address, parts[1], configs.Config).Wait();
            }
        }
    }
}
