using Microsoft.Extensions.Configuration;
using Serilog;

namespace Common.Configuration
{
    /// <summary>
    /// Provides methods for configuring Serilog logging.
    /// </summary>
    public static class SerilogConfiguration
    {
        /// <summary>
        /// Creates and configures a Serilog logger instance.
        /// </summary>
        /// <param name="configuration">The application's configuration settings.</param>
        /// <param name="appName">The name of the application to use in log entries.</param>
        /// <returns>A configured <see cref="ILogger"/> instance for Serilog.</returns>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="configuration"/> or <paramref name="appName"/> is null.</exception>
        public static ILogger CreateSerilogLogger(IConfiguration configuration, string appName) =>
            configuration switch
            {
                null => throw new ArgumentNullException(nameof(configuration)),
                _ => appName switch
                {
                    null => throw new ArgumentNullException(nameof(appName)),
                    _ => new LoggerConfiguration()
                                .MinimumLevel.Verbose()
                                .Enrich.WithProperty("ApplicationContext", appName)
                                .Enrich.FromLogContext()
                                .WriteTo.Console()
                                .ReadFrom.Configuration(configuration)
                                .CreateLogger(),
                },
            };
    }
}