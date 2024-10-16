#pragma warning disable CS1998, CA1062

namespace VendorConfiguration.Infrastructure
{
    /// <summary>
    /// Provides extension methods for the <see cref="IMediator"/> interface to dispatch domain events.
    /// </summary>
    public static class MediatorExtension
    {
        /// <summary>
        /// Dispatches domain events for all entities tracked by the provided <see cref="VendorConfigurationContext"/> and clears the events after dispatching.
        /// </summary>
        /// <param name="mediator">The mediator used to dispatch the domain events.</param>
        /// <param name="ctx">The database context tracking entities with domain events.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mediator"/> or <paramref name="ctx"/> is null.</exception>
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, VendorConfigurationContext ctx, CancellationToken? cancellationToken = default)
        {
            ArgumentGuard.NotNull(mediator, nameof(mediator));
            ArgumentGuard.NotNull(ctx, nameof(ctx));

            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0);

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            // Clear the domain events from the entities after retrieving them
            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            // Publish each domain event using the mediator
            domainEvents.ForEach(async domainEvent => await mediator.Publish(domainEvent, cancellationToken ?? CancellationToken.None).ConfigureAwait(false));
        }

        /// <summary>
        /// Dispatches domain events for a single entity and clears the events after dispatching.
        /// </summary>
        /// <param name="mediator">The mediator used to dispatch the domain events.</param>
        /// <param name="entity">The entity that has domain events to dispatch.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mediator"/> or <paramref name="entity"/> is null.</exception>
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, Entity entity, CancellationToken? cancellationToken = default)
        {
            ArgumentGuard.NotNull(mediator, nameof(mediator));
            ArgumentGuard.NotNull(entity, nameof(entity));

            var notifications = entity.DomainEvents.ToList<INotification>();

            // Publish each domain event using the mediator
            notifications.ForEach(async domainEvent => await mediator.Publish(domainEvent, cancellationToken ?? CancellationToken.None).ConfigureAwait(false));
        }
    }
}
