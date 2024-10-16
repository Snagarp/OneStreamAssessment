namespace VendorConfiguration.Infrastructure.Idempotency
{
    /// <summary>
    /// Provides an interface for managing client requests to ensure idempotency, preventing duplicate processing of commands.
    /// </summary>
    public interface IRequestManager
    {
        /// <summary>
        /// Checks if a request with the specified unique identifier already exists.
        /// </summary>
        /// <param name="id">The unique identifier of the request.</param>
        /// <returns>A task representing the asynchronous operation. The task result is true if the request exists, otherwise false.</returns>
        Task<bool> ExistAsync(Guid id);

        /// <summary>
        /// Creates a record of a request for a specific command, identified by a unique identifier, to ensure the command is processed only once.
        /// </summary>
        /// <typeparam name="T">The type of the command for which the request is created.</typeparam>
        /// <param name="id">The unique identifier of the request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
