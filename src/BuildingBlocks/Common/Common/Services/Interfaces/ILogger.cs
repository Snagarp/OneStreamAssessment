namespace Common.Services.Interfaces
{
    /// <summary>
    /// Defines logging functionality for capturing information, warnings, and errors.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogInformation(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs an error message along with an associated exception.
        /// </summary>
        /// <param name="exception">The exception related to the error.</param>
        /// <param name="message">The error message to log.</param>
        void LogError(Exception exception, string message);
    }
}