//2024 (c) TD Synnex - All Rights Reserved.

using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Graylog;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Common.Services
{
    public class LoggingService : Interfaces.ILogger
    {
        private readonly Serilog.ILogger _logger;

        public LoggingService(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Configuration parameter cannot be null.");
            }
            var appName = configuration["Application:Name"] ?? "DefaultAppName";
            var environment = configuration["Application:Environment"] ?? "Development";

            _logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("applicationName", appName)
                .Enrich.WithProperty("environment", environment)
                .Enrich.FromLogContext()
                .WriteTo.Graylog(new GraylogSinkOptions
                {
                    HostnameOrAddress = configuration["Graylog:Hostname"],
                    Port = int.Parse(configuration["Graylog:Port"] ?? "0", CultureInfo.InvariantCulture),

                })
                .CreateLogger();
        }

        void Interfaces.ILogger.LogError(Exception exception, string message)
        {
            _logger.Error(exception, message);
        }

        void Interfaces.ILogger.LogInformation(string message)
        {
            _logger.Information(message);
        }

        void Interfaces.ILogger.LogWarning(string message)
        {
            _logger.Warning(message);
        }
    }
}
