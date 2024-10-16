using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Domain.Events
{
    /// <summary>
    /// Represents a domain event that occurs when a <see cref="Country"/> entity is deleted.
    /// </summary>
    public class DeleteCountryEvent : CountryDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCountryEvent"/> class.
        /// </summary>
        /// <param name="country">The <see cref="Country"/> entity that is being deleted.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="country"/> is null.</exception>
        public DeleteCountryEvent(Country country) :
            base(ArgumentGuard.NotNull(country))
        {
        }
    }
}
