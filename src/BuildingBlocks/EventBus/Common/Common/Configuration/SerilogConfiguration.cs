//2023 (c) TD Synnex - All Rights Reserved.



using Microsoft.Extensions.Configuration;

using Serilog;

namespace Common.Configuration
{
    public static class SerilogConfiguration
    {
        public static ILogger CreateSerilogLogger(IConfiguration configuration, string appName)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));
            if (appName is null) throw new ArgumentNullException(nameof(appName));

            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
