using Serilog;

namespace FW.Management.Configurations
{
    public static class LoggerConfiguration
    {
        public static void ConfigureLogger(this IServiceCollection services)
        {
            string UrlSeq = "http://db_seq:5341";

            Log.Logger = new Serilog.LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Seq(UrlSeq)
                .CreateLogger();

            services.AddSingleton(Log.Logger);
        }
    }
}
