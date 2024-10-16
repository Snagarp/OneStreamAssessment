using Application.DTO.Request.Country;
using Application.DTO.Response;
using Application.DTO.Response.Country;
using Application.Extensions;
using System.Net.Http.Json;
using System.Text.Json;

namespace Application.Service
{
    public class CountryService(HttpClientService httpClientService) : ICountryService
    {
        /// <summary>
        /// Retrieves the HTTP client for making private API calls.
        /// </summary>
        /// <returns>An instance of <see cref="HttpClient"/> configured for private API calls.</returns>
        private async Task<HttpClient> PrivateClient() => await httpClientService.GetPrivateClient();

        /// <summary>
        /// Checks the status of an HTTP response and returns an error message if the response indicates a failure.
        /// </summary>
        /// <param name="response">The HTTP response to check.</param>
        /// <returns>A string error message if the response indicates an error; otherwise, <c>null</c>.</returns>
        private static string? CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return $"Sorry, an unknown error occurred.{Environment.NewLine}Error Description:{Environment.NewLine}Status Code: {response.StatusCode}{Environment.NewLine}Reason Phrase: {response.ReasonPhrase}";
            else
                return null;
        }

        /// <summary>
        /// Creates a standardized error response.
        /// </summary>
        /// <param name="message">The error message to include in the response.</param>
        /// <returns>A <see cref="Response"/> object indicating failure.</returns>
        private static Response ErrorOperation(string message) => new(false, message);

        // Add
        /// <summary>
        /// Adds a new Country.
        /// </summary>
        /// <param name="model">The details of the country to add.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>       
        public async Task<Response> AddCountry(CreateCountryRequestDTO countryModel)
        {
            HttpResponseMessage result = await (await PrivateClient()).PostAsJsonAsync(Constant.AddCountryRoute, countryModel);
            string? error = CheckResponseStatus(result);
            if (!string.IsNullOrEmpty(error)) return ErrorOperation(error);
            return await result.Content.ReadFromJsonAsync<Response>() ?? ErrorOperation("Failed to add Country.");
        }

        // Get / List
        /// <summary>
        /// Retrieves a list of all Countries.
        /// </summary>
        /// <param name="jwtToken">The JWT token used for authorization.</param>
        /// <returns>An <see cref="IEnumerable{GetCountriesResponseDTO}"/> containing all countries.</returns>       
        public async Task<IEnumerable<GetCountriesResponseDTO>> GetCountries(string jwtToken)
        {
            try
            {
                // Create an HttpClient instance using the configured PrivateClient.
                using var client = await PrivateClient();

                // Set up the request to call the API.
                var request = new HttpRequestMessage(HttpMethod.Get, Constant.GetCountriesRoute);

                // Add the required headers.
                request.Headers.Add("JwtToken", jwtToken);            

                // Send the request and get the response.
                var response = await client.SendAsync(request);

                // Ensure the request was successful.
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: Unable to retrieve countries. Status code: {response.StatusCode}");
                    return [];
                }

                // Deserialize the response content to a list of GetCountriesResponseDTO.
                var countries = await response.Content.ReadFromJsonAsync<IEnumerable<GetCountriesResponseDTO>>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
               

                return countries ?? [];
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP error: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON parsing error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            // Return an empty list in case of any errors.
            return [];
        }


    }
}
