#pragma warning disable CS8618, CA1062, CA1707, CA1303, S4136

using Microsoft.Extensions.Logging;
using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Infrastructure
{
    /// <summary>
    /// Represents the Entity Framework database context for the VendorConfiguration system, handling entity tracking and database transactions.
    /// </summary>
    public class VendorConfigurationContext : DbContext, IUnitOfWork
    {
        public const string DEFAULTSCHEMA = "dbo";
        private readonly IMediator _mediator;
        private readonly ILogger<VendorConfigurationContext> _logger;
        private IDbContextTransaction? _currentTransaction;

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="Country"/> entities.
        /// </summary>
        public DbSet<Country> Countries { get; set; }

        /// <summary>
        /// Gets the current active transaction for the context.
        /// </summary>
        /// <returns>The current <see cref="IDbContextTransaction"/>.</returns>
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction!;

        /// <summary>
        /// Indicates whether the context has an active transaction.
        /// </summary>
        public bool HasActiveTransaction => _currentTransaction is not null;

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorConfigurationContext"/> class with the specified options, mediator, and logger.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        /// <param name="mediator">The mediator for dispatching domain events.</param>
        /// <param name="logger">The logger for logging context operations.</param>
        public VendorConfigurationContext(DbContextOptions<VendorConfigurationContext> options, IMediator mediator, ILogger<VendorConfigurationContext> logger) : base(options)
        {
            ArgumentGuard.NotNull(options, nameof(options));
            _mediator = ArgumentGuard.NotNull(mediator, nameof(mediator));
            _logger = ArgumentGuard.NotNull(logger, nameof(logger));
            System.Diagnostics.Debug.WriteLine("ConfigurationContext::ctor ->" + this.GetHashCode());
        }

        /// <summary>
        /// Configures the model for the context.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the entity model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentGuard.NotNull(modelBuilder, nameof(modelBuilder));
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
        }

        /// <summary>
        /// Saves all changes made in the context to the database and dispatches domain events after committing.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token for the operation.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the save was successful.</returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            ArgumentGuard.NotNull(cancellationToken, nameof(cancellationToken));

            var entries = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified || e.State == EntityState.Added)
                .ToList();

            foreach (var entry in entries)
            {
                _logger.LogInformation("Entity: {Name}, State: {State}", entry.Entity.GetType().Name, entry.State);
            }

            await base.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            _logger.LogInformation("Clearing change tracker after commit");
            this.ChangeTracker.Clear();

            _logger.LogInformation("Dispatching domain events after commit");
            await (_mediator.DispatchDomainEventsAsync(this)).ConfigureAwait(true)!;

            return true;
        }

        /// <summary>
        /// Begins a database transaction with read-committed isolation level.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning the started <see cref="IDbContextTransaction"/>.</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(true);
            return _currentTransaction;
        }

        /// <summary>
        /// Commits the current database transaction.
        /// </summary>
        /// <param name="transaction">The transaction to commit.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the transaction is not the current one.</exception>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            ArgumentGuard.NotNull(transaction, nameof(transaction));

            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync().ConfigureAwait(true);
                await transaction.CommitAsync().ConfigureAwait(true);
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
