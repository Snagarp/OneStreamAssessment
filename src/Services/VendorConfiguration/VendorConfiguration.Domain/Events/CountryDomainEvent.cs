using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Domain.Events
{
    /// <summary>
    /// Represents an abstract base class for domain events related to the <see cref="Country"/> entity.
    /// </summary>
    public abstract class CountryDomainEvent : INotification
    {
        /// <summary>
        /// Gets the <see cref="Country"/> entity associated with the domain event.
        /// </summary>
        public Country Country { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryDomainEvent"/> class.
        /// </summary>
        /// <param name="country">The <see cref="Country"/> entity that the event is related to.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="country"/> is null.</exception>
        protected CountryDomainEvent(Country country) =>
            Country = ArgumentGuard.NotNull(country);
    }
}
