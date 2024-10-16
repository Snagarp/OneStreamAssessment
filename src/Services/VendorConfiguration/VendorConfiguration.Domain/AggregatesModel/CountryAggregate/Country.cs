#pragma warning disable CS8618

namespace VendorConfiguration.Domain.AggregatesModel.CountryAggregate
{
    /// <summary>
    /// Represents a country entity in the domain model.
    /// </summary>
    public class Country : Entity, IAggregateRoot
    {
        /// <summary>
        /// Gets or sets the unique identifier for the country.
        /// </summary>
        public int CountryId
        {
            get { return Id; }
            set { Id = value; }
        }

        /// <summary>
        /// Gets or sets the ISO 3166-1 alpha-2 country code (2-letter code).
        /// </summary>
        public string Iso3166CountryCode2 { get; set; }

        /// <summary>
        /// Gets or sets the ISO 3166-1 alpha-3 country code (3-letter code).
        /// </summary>
        public string Iso3166CountryCode3 { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        public Country() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class with the specified country codes and name.
        /// </summary>
        /// <param name="iso31662CountryCode">The ISO 3166-1 alpha-2 country code.</param>
        /// <param name="iso31663CountryCode">The ISO 3166-1 alpha-3 country code.</param>
        /// <param name="countryName">The name of the country.</param>
        public Country(string iso31662CountryCode, string iso31663CountryCode, string countryName)
        {
            Iso3166CountryCode2 = ArgumentGuard.NotNullOrEmpty(iso31662CountryCode, nameof(iso31662CountryCode));
            Iso3166CountryCode3 = ArgumentGuard.NotNullOrEmpty(iso31663CountryCode, nameof(iso31663CountryCode));
            CountryName = ArgumentGuard.NotNullOrEmpty(countryName, nameof(countryName));

            AddNewDomainEvent();
        }

        /// <summary>
        /// Adds a domain event indicating the country has been modified.
        /// </summary>
        public void Modify() => AddModifyDomainEvent();

        /// <summary>
        /// Adds a domain event indicating the country has been deleted.
        /// </summary>
        public void Delete() => this.AddDomainEvent(new DeleteCountryEvent(this));

        /// <summary>
        /// Adds a domain event for a newly created country.
        /// </summary>
        private void AddNewDomainEvent() => this.AddDomainEvent(new NewCountryEvent(this));

        /// <summary>
        /// Adds a domain event for a modified country.
        /// </summary>
        private void AddModifyDomainEvent() => this.AddDomainEvent(new ModifyCountryEvent(this));
    }
}
