using Microsoft.Extensions.Hosting;
using Serilog;

namespace WebAPI.Extensions
{
    public static class LoggingExtensions
    {
        public static void ConfigureSerilog(IHostBuilder host)
        {
            host.UseSerilog((context, configuration) =>
            {
                configuration
                .WriteTo.Console()
                .MinimumLevel.Information();
            });
        }
    }
}
