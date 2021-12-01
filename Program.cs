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
using Microsoft.Extensions.DependencyInjection;
using P2P.Utility;
using System.Net.Http;

namespace P2P
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configsTmp = new ConfigLoader();
            var host = CreateHostBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                    builder.UseUrls($"https://localhost:{configsTmp.Config.NodePort}");
                }).Build();

            using var scope = host.Services.CreateScope();
            var configs = scope.ServiceProvider.GetRequiredService<IConfigLoader>();
            var httpClient = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>()
                .CreateClient();
            
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Terminal.TerminalHandler.Run(configs, httpClient);
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
