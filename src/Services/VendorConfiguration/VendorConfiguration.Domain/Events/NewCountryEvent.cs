using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Domain.Events
{
    /// <summary>
    /// Represents a domain event that occurs when a new <see cref="Country"/> entity is created.
    /// </summary>
    public class NewCountryEvent : CountryDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewCountryEvent"/> class.
        /// </summary>
        /// <param name="country">The new <see cref="Country"/> entity that has been created.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="country"/> is null.</exception>
        public NewCountryEvent(Country country) :
            base(ArgumentGuard.NotNull(country))
        {
        }
    }
}
