namespace VendorConfiguration.Infrastructure.Idempotency
{
    /// <summary>
    /// Provides implementation for the <see cref="IRequestManager"/> interface to manage idempotent client requests and prevent duplicate processing of commands.
    /// </summary>
    public class RequestManager : IRequestManager
    {
        private readonly VendorConfigurationContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestManager"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The <see cref="VendorConfigurationContext"/> used to manage client requests.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context"/> is null.</exception>
        public RequestManager(VendorConfigurationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Checks if a client request with the specified unique identifier already exists in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the client request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the request exists, otherwise false.</returns>
        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context.FindAsync<ClientRequest>(id);
            return request != null;
        }

        /// <summary>
        /// Creates a new client request for a specific command if it does not already exist. 
        /// Ensures that the command is processed only once.
        /// </summary>
        /// <typeparam name="T">The type of the command associated with the client request.</typeparam>
        /// <param name="id">The unique identifier of the client request.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown if a request with the same <paramref name="id"/> already exists.</exception>
        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new Exception($"Request with {id} already exists") :
                new ClientRequest(id, typeof(T).Name, DateTime.UtcNow);

            _context.Add(request);
            await _context.SaveChangesAsync();
        }
    }
}
