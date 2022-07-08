using DocumentsExternal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace DocumentsExternal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddUserSecrets<DocumentsConfiguration>()
                    .Build();

                var options = configuration
                    .GetSection(nameof(DocumentsConfiguration))
                    .Get<DocumentsConfiguration>();

                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Seq(options.SeqHttpHost, apiKey: options.SeqApiKey)
                    .CreateLogger();

                Log.Information("Запуск сервиса.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Работа сервиса остановлена.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                    builder.AddSerilog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}