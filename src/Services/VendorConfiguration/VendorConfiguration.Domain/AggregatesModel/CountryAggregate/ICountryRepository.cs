namespace VendorConfiguration.Domain.AggregatesModel.CountryAggregate
{
    /// <summary>
    /// Defines the contract for a repository that manages <see cref="Country"/> entities.
    /// </summary>
    public interface ICountryRepository
    {
        /// <summary>
        /// Gets the unit of work associated with the repository, used to track changes.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Retrieves all country entities.
        /// </summary>
        /// <param name="stoppingToken">A token to observe cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="Country"/> entities.</returns>
        Task<IEnumerable<Country>> GetAll(CancellationToken stoppingToken);

        /// <summary>
        /// Retrieves a country entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the country.</param>
        /// <param name="stoppingToken">A token to observe cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Country"/> entity, or null if not found.</returns>
        Task<Country?> GetById(int id, CancellationToken stoppingToken);

        /// <summary>
        /// Retrieves a country entity by its ISO country code.
        /// </summary>
        /// <param name="countryCode">The ISO country code (either 2 or 3 characters).</param>
        /// <param name="stoppingToken">A token to observe cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Country"/> entity, or null if not found.</returns>
        Task<Country?> GetByCountryCode(string countryCode, CancellationToken stoppingToken);

        /// <summary>
        /// Adds a new country entity to the repository.
        /// </summary>
        /// <param name="country">The country entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added <see cref="Country"/> entity.</returns>
        Task<Country> Add(Country country);

        /// <summary>
        /// Updates an existing country entity in the repository.
        /// </summary>
        /// <param name="country">The country entity to update.</param>
        void Update(Country country);

        /// <summary>
        /// Deletes an existing country entity from the repository.
        /// </summary>
        /// <param name="country">The country entity to delete.</param>
        void Delete(Country country);

        /// <summary>
        /// Determines if a country with the specified ISO codes is unique in the repository.
        /// </summary>
        /// <param name="iso2Char">The ISO 3166-1 alpha-2 country code (2 characters).</param>
        /// <param name="iso3Char">The ISO 3166-1 alpha-3 country code (3 characters).</param>
        /// <param name="stoppingToken">A token to observe cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the country is unique, otherwise false.</returns>
        Task<bool> IsCountryUnique(string iso2Char, string iso3Char, CancellationToken stoppingToken = default);

        /// <summary>
        /// Determines if a country with the specified ISO codes is unique during modification.
        /// </summary>
        /// <param name="country">The country entity being modified.</param>
        /// <param name="iso2Char">The ISO 3166-1 alpha-2 country code (2 characters).</param>
        /// <param name="iso3Char">The ISO 3166-1 alpha-3 country code (3 characters).</param>
        /// <param name="stoppingToken">A token to observe cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the country is unique, otherwise false.</returns>
        Task<bool> IsCountryUniqueOnModify(Country country, string iso2Char, string iso3Char, CancellationToken stoppingToken);
    }
}
