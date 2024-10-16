namespace Identity.Security
{
    /// <summary>
    /// Interface for the Logger Helper
    /// </summary>
    public interface ILoggerHelper
    {
        /// <summary>
        /// It will append correlationId along with message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>string</returns>
        string LogWithCorrelationId(string message);
    }
}
