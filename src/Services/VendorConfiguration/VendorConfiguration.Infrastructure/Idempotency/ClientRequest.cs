namespace VendorConfiguration.Infrastructure.Idempotency
{
    /// <summary>
    /// Represents a client request with idempotency properties to ensure unique request processing.
    /// </summary>
    public class ClientRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the client request.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the client request.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the client request was made.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRequest"/> class with the specified id, name, and time.
        /// </summary>
        /// <param name="id">The unique identifier for the request.</param>
        /// <param name="name">The name of the request.</param>
        /// <param name="time">The timestamp of the request.</param>
        public ClientRequest(Guid id, string name, DateTime time)
        {
            Id = id;
            Name = name;
            Time = time;
        }
    }
}
