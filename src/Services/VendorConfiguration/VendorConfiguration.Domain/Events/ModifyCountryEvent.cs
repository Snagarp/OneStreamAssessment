using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Domain.Events
{
    /// <summary>
    /// Represents a domain event that occurs when a <see cref="Country"/> entity is modified.
    /// </summary>
    public class ModifyCountryEvent : CountryDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyCountryEvent"/> class.
        /// </summary>
        /// <param name="country">The <see cref="Country"/> entity that has been modified.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="country"/> is null.</exception>
        public ModifyCountryEvent(Country country) :
            base(ArgumentGuard.NotNull(country))
        {
        }
    }
}
