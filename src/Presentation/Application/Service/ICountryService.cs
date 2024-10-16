
using Application.DTO.Request.Country;
using Application.DTO.Response;
using Application.DTO.Response.Country;

namespace Application.Service
{
    public interface ICountryService
    {
        /// <summary>
        /// Adds a new country to the system.
        /// </summary>
        /// <param name="model">The country creation details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the country addition.</returns>
        Task<Response> AddCountry(CreateCountryRequestDTO countryModel);

        /// <summary>
        /// Retrieves a list of all countries.
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns>A list of countries as <see cref="GetCountriesResponseDTO"/> objects.</returns>
        Task<IEnumerable<GetCountriesResponseDTO>> GetCountries(string jwtToken);
    }
}
