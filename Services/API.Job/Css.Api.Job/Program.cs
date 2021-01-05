using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Job
{
    /// <summary>
    /// Program starts here
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureAppConfiguration((context, builder) =>
                        {
                            var env = context.HostingEnvironment;

                            builder.AddJsonFile("appsettings.json",
                                         optional: true, reloadOnChange: true)
                                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                                         optional: true, reloadOnChange: true)
                                   .AddJsonFile(Path.Combine("Configuration", "configuration.json"),
                                         optional: true, reloadOnChange: true)
                                   .AddJsonFile(Path.Combine("Configuration", $"configuration.{env.EnvironmentName}.json"), 
                                         optional: true, reloadOnChange: true)
                                   .AddEnvironmentVariables()
                                   .Build();
                        }).UseSerilog();
                });
    }
}
