using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Infrastructure.Repositories
{
    /// <summary>
    /// Provides the repository implementation for managing <see cref="Country"/> entities in the database.
    /// </summary>
    public class CountryRepository : ICountryRepository
    {
        private readonly VendorConfigurationContext _context;

        /// <summary>
        /// Gets the unit of work associated with the repository for tracking changes.
        /// </summary>
        public IUnitOfWork UnitOfWork => _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context used for managing <see cref="Country"/> entities.</param>
        public CountryRepository(VendorConfigurationContext context) =>
            _context = ArgumentGuard.NotNull(context);

        /// <summary>
        /// Retrieves all countries from the database.
        /// </summary>
        /// <param name="stoppingToken">A cancellation token for the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of all <see cref="Country"/> entities.</returns>
        public async Task<IEnumerable<Country>> GetAll(CancellationToken stoppingToken) =>
            await _context.Countries
                .AsNoTracking()
                .ToListAsync(stoppingToken).ConfigureAwait(true);

        /// <summary>
        /// Retrieves a country by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the country.</param>
        /// <param name="stoppingToken">A cancellation token for the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Country"/> entity if found, otherwise null.</returns>
        public async Task<Country?> GetById(int id, CancellationToken stoppingToken) =>
            await _context.Countries
                .FirstOrDefaultAsync(X => X.CountryId == id, stoppingToken).ConfigureAwait(true);

        /// <summary>
        /// Retrieves a country by its ISO country code.
        /// </summary>
        /// <param name="countryCode">The ISO country code (either 2 or 3 characters).</param>
        /// <param name="stoppingToken">A cancellation token for the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Country"/> entity if found, otherwise null.</returns>
        public async Task<Country?> GetByCountryCode(string countryCode, CancellationToken stoppingToken)
        {
            ArgumentGuard.NotNullOrEmpty(countryCode, nameof(countryCode));

            return countryCode.Length switch
            {
                2 => await _context.Countries
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Iso3166CountryCode2 == countryCode, stoppingToken).ConfigureAwait(false),
                3 => await _context.Countries
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Iso3166CountryCode3 == countryCode, stoppingToken).ConfigureAwait(false),
                _ => null,
            };
        }

        /// <summary>
        /// Checks if a country with the specified ISO codes is unique.
        /// </summary>
        /// <param name="iso2Char">The ISO 3166-1 alpha-2 country code (2 characters).</param>
        /// <param name="iso3Char">The ISO 3166-1 alpha-3 country code (3 characters).</param>
        /// <param name="stoppingToken">A cancellation token for the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the country is unique, otherwise false.</returns>
        public async Task<bool> IsCountryUnique(string iso2Char, string iso3Char, CancellationToken stoppingToken = default)
        {
            ArgumentGuard.NotNullOrEmpty(iso2Char, nameof(iso2Char));
            ArgumentGuard.NotNullOrEmpty(iso3Char, nameof(iso3Char));
            var x = await _context.Countries.FirstOrDefaultAsync(x => x.Iso3166CountryCode2 == iso2Char || x.Iso3166CountryCode3 == iso3Char, stoppingToken).ConfigureAwait(true);
            return x is null;
        }

        /// <summary>
        /// Checks if a country with the specified ISO codes is unique during modification.
        /// </summary>
        /// <param name="country">The country being modified.</param>
        /// <param name="iso2Char">The ISO 3166-1 alpha-2 country code (2 characters).</param>
        /// <param name="iso3Char">The ISO 3166-1 alpha-3 country code (3 characters).</param>
        /// <param name="stoppingToken">A cancellation token for the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the country is unique or matches the country being modified, otherwise false.</returns>
        public async Task<bool> IsCountryUniqueOnModify(Country country, string iso2Char, string iso3Char, CancellationToken stoppingToken)
        {
            ArgumentGuard.NotNull(country, nameof(country));

            var x = await _context.Countries.FirstOrDefaultAsync(x => x.Iso3166CountryCode2 == iso2Char || x.Iso3166CountryCode3 == iso3Char, stoppingToken).ConfigureAwait(true);

            return x switch
            {
                null => true,
                _ => x.CountryId == country.CountryId,
            };
        }

        /// <summary>
        /// Adds a new country entity to the database.
        /// </summary>
        /// <param name="country">The country entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the added <see cref="Country"/> entity.</returns>
        public async Task<Country> Add(Country country)
        {
            await _context.Countries.AddAsync(country).ConfigureAwait(true);
            return country;
        }

        /// <summary>
        /// Updates an existing country entity in the database.
        /// </summary>
        /// <param name="country">The country entity to update.</param>
        public void Update(Country country) =>
            _context.Entry(country).State = EntityState.Modified;

        /// <summary>
        /// Deletes a country entity from the database.
        /// </summary>
        /// <param name="country">The country entity to delete.</param>
        public void Delete(Country country) =>
            _context.Entry(country).State = EntityState.Deleted;
    }
}
