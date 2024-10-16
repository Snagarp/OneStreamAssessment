using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Graylog;
using Serilog;
using System.Globalization;

namespace Common.Services
{
    /// <summary>
    /// Logging service that uses Serilog for logging information, warnings, and errors.
    /// Logs are sent to Graylog.
    /// </summary>
    public class LoggingService : Interfaces.ILogger
    {
        private readonly Serilog.ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration, used to configure logging.</param>
        /// <exception cref="ArgumentNullException">Thrown when the configuration is null.</exception>
        public LoggingService(IConfiguration configuration)
        {
            switch (configuration)
            {
                case null:
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

        /// <summary>
        /// Logs an error message along with an exception.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The error message to log.</param>
        void Interfaces.ILogger.LogError(Exception exception, string message) => _logger.Error(exception, message);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        void Interfaces.ILogger.LogInformation(string message) => _logger.Information(message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        void Interfaces.ILogger.LogWarning(string message) => _logger.Warning(message);
    }
}
