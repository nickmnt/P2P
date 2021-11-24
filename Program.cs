using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using P2P.Models;

namespace P2P
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //Read Yaml
            var configYml = File.ReadAllText("Config.yml");
            var nodeFilesYml = File.ReadAllText("NodeFiles.yml");

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var config = deserializer.Deserialize<Config>(configYml);
            var nodeFilesInfo = deserializer.Deserialize<NodeFilesInfo>(nodeFilesYml);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Terminal.TerminalHandler.Run();
            }).Start();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
