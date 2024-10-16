using Microsoft.AspNetCore.Http;

namespace Identity.Security
{
    /// <summary>
    /// Provides helper methods for logging custom information with additional metadata such as correlation IDs.
    /// </summary>
    public class LoggerHelper : ILoggerHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerHelper"/> class.
        /// </summary>
        /// <param name="context">The <see cref="IHttpContextAccessor"/> used to access the current HTTP context.</param>
        public LoggerHelper(IHttpContextAccessor context)
        {
            _httpContextAccessor = context;
        }

        /// <summary>
        /// Appends the correlation ID from the HTTP request headers to the provided log message.
        /// </summary>
        /// <param name="message">The original log message to be logged.</param>
        /// <returns>
        /// A string containing the correlation ID (if available) and the original log message.
        /// If the correlation ID is not present, only the original message is returned.
        /// </returns>
        public string LogWithCorrelationId(string message)
        {
            if (_httpContextAccessor?.HttpContext != null)
            {
                var correlationId = _httpContextAccessor.HttpContext.Request.Headers["os-correlationid"].ToString();
                return $"os-correlationid:{correlationId},{message}";
            }
            return message;
        }
    }
}