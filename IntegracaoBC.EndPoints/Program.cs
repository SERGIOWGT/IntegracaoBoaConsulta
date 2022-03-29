using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IntegracaoBC.EndPoints
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseIISIntegration();
                })
              .UseSerilog((hostContext, services, logger) => {
                  logger.ReadFrom.Configuration(hostContext.Configuration);
                  logger.WriteTo.File("logs/all-.logs", rollingInterval: RollingInterval.Day);
              });
    }
}
