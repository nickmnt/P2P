using P2P.Models;
using P2P.Models.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace P2P.Utility
{
    public static class HttpExtensions
    {
        public static async Task DownloadFile(this HttpClient httpClient, int name, int port, string fileName, Config config)
        {
                var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://localhost:{port}/file/download/{fileName}/{config.NodeNumber}/{config.NodePort}");

                Console.WriteLine($"Sending request to node {name} on port {port} for file {fileName}");
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                Console.WriteLine($"Response for the request to port {port} for file {fileName} received");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Node {name} with port {port} has sent the file {fileName} as response");
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();

                    using var fileStream =
                        File.Create($"./{config.NewFilesDir}{fileName}");

                    contentStream.Seek(0, SeekOrigin.Begin);
                    contentStream.CopyTo(fileStream);

                Console.WriteLine("File successfuly written to " + $"./{config.NewFilesDir}{fileName}");
                }
        }

        public static async Task<string> RequestAddress(this HttpClient httpClient, int target, Config config, List<int> visited, int name, int port)
        {
            do
            {
                if (visited.Contains(name))
                    break;

                var content = new AddressRequestDto
                {
                    RequesterName = config.NodeNumber,
                    TargetName = target,
                    Visited = visited
                };

                var json = new StringContent(
                    JsonSerializer.Serialize(content),
                    Encoding.UTF8,
                    Application.Json);

                Console.WriteLine($"Sending request to node {name} with port {port} for {target}'s address.");
                using var httpResponseMessage =
                    await httpClient.PostAsync($"https://localhost:{port}/address/find/", json);

                httpResponseMessage.EnsureSuccessStatusCode();

                Console.WriteLine($"Response received from node {name} with port {port} for {target}'s address.");
                var msg = await httpResponseMessage.Content.ReadAsStringAsync();
                visited.Add(name);

                if (msg == "empty")
                {
                    break;
                }

                var parts = msg.Split(' ');
                if (parts[0] == "success")
                {
                    return parts[1];
                }
                else if (parts[0] == "try")
                {
                    var result = await RequestAddress(httpClient, target,
                        config, visited, Int32.Parse(parts[1]), Int32.Parse(parts[2]));
                    
                    if(result != "empty") {
                        return result;
                    }
                }

            } while (true);

            return "empty";
        }

        public static async Task<int> FindAddress(this HttpClient httpClient, int target, Config config)
        {
            var neighbors = new List<Node>();
            foreach(var n in config.FriendNodes)
            {
                neighbors.Add(n);
            }
            neighbors.Sort( (x,y) =>
            {
                return Math.Abs(x.NodeName - target).CompareTo(Math.Abs(x.NodeName - target));
            });

            var visited = new List<int>();
            visited.Add(config.NodeNumber);

            int port = -1;
            foreach(var neighbor in neighbors)
            {
                var result = await httpClient.RequestAddress(target, config, visited, neighbor.NodeName, neighbor.NodePort);
                if(result != "empty")
                {
                    port = Int32.Parse(result);
                    break;
                }
            }

            return port;
        }
    }
}
