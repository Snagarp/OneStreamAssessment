namespace VendorConfiguration.Infrastructure.Repositories
{
    /// <summary>
    /// Provides a base repository implementation with shared functionality for interacting with the database context.
    /// </summary>
    public abstract class BaseRepository
    {
        /// <summary>
        /// Gets the <see cref="VendorConfigurationContext"/> used to interact with the database.
        /// </summary>
        protected VendorConfigurationContext Context { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The <see cref="VendorConfigurationContext"/> used for database operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context"/> is null.</exception>
        protected BaseRepository(VendorConfigurationContext context) =>
            Context = ArgumentGuard.NotNull(context, nameof(context));

        /// <summary>
        /// Gets the <see cref="IUnitOfWork"/> that tracks changes and coordinates saving changes to the database.
        /// </summary>
        public IUnitOfWork UnitOfWork => Context;
    }
}
