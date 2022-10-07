using Serilog;
using FW.LogOptions;
namespace FW.Management.Configurations;

public static class LoggerConfiguration
{
    public static void ConfigureLogger(this IServiceCollection services, IConfiguration configuration)
    {
        var logger_options = configuration.GetSection(LoggerOptions.KeyValue).Get<LoggerOptions>();

        Log.Logger = new Serilog.LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Seq(logger_options.Url)
            .CreateLogger();

        services.AddSingleton(Log.Logger);
    }
}
