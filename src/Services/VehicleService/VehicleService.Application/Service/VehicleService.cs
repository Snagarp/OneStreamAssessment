#pragma warning disable IDE0301
using System.Net.Http.Json;
using Vehicle.Application.Extensions;
using Vehicle.Application.DTOs.Request.Vehicles;
using Vehicle.Application.DTOs.Response.Vehicles;
using Vehicle.Application.DTOs.Response;

namespace Vehicle.Application.Service
{
    /// <summary>
    /// Service for handling vehicle-related operations, including adding, updating, retrieving, and deleting vehicles, brands, and owners.
    /// </summary>
    public class VehicleService(HttpClientService httpClientService) : IVehicleService
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
        private static string? CheckResponseStatus(HttpResponseMessage response) => response.IsSuccessStatusCode switch
        {
            false => $"Sorry, an unknown error occurred.{Environment.NewLine}Error Description:{Environment.NewLine}Status Code: {response.StatusCode}{Environment.NewLine}Reason Phrase: {response.ReasonPhrase}",
            _ => null,
        };

        /// <summary>
        /// Creates a standardized error response.
        /// </summary>
        /// <param name="message">The error message to include in the response.</param>
        /// <returns>A <see cref="Response"/> object indicating failure.</returns>
        private static Response ErrorOperation(string message) => new(false, message);

        // Add
        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="model">The details of the vehicle to add.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>
        public async Task<Response> AddVehicle(CreateVehicleRequestDTO model)
        {
            var result = await (await PrivateClient()).PostAsJsonAsync(Constant.AddVehicleRoute, model);
            string? error = CheckResponseStatus(result);
            return string.IsNullOrEmpty(error) switch
            {
                false => ErrorOperation(error),
                _ => await result.Content.ReadFromJsonAsync<Response>() ?? ErrorOperation("Failed to add vehicle."),
            };
        }

        /// <summary>
        /// Adds a new vehicle brand.
        /// </summary>
        /// <param name="model">The details of the vehicle brand to add.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>
        public async Task<Response> AddVehicleBrand(CreateVehicleBrandRequestDTO model)
        {
            var result = await (await PrivateClient()).PostAsJsonAsync(Constant.AddVehicleBrandRoute, model);
            string? error = CheckResponseStatus(result);
            return string.IsNullOrEmpty(error) switch
            {
                false => ErrorOperation(error),
                _ => await result.Content.ReadFromJsonAsync<Response>() ?? ErrorOperation("Failed to add vehicle brand."),
            };
        }

        /// <summary>
        /// Adds a new vehicle owner.
        /// </summary>
        /// <param name="model">The details of the vehicle owner to add.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>
        public async Task<Response> AddVehicleOwner(CreateVehicleOwnerRequestDTO model)
        {
            var result = await (await PrivateClient()).PostAsJsonAsync(Constant.AddVehicleOwnerRoute, model);
            string? error = CheckResponseStatus(result);
            return string.IsNullOrEmpty(error) switch
            {
                false => ErrorOperation(error),
                _ => await result.Content.ReadFromJsonAsync<Response>() ?? ErrorOperation("Failed to add vehicle owner."),
            };
        }

        // Delete
        /// <summary>
        /// Deletes a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>
        public async Task<Response> DeleteVehicle(int id)
        {
            var result = await (await PrivateClient()).DeleteAsync($"{Constant.DeleteVehicleRoute}/{id}");
            string? error = CheckResponseStatus(result);
            return string.IsNullOrEmpty(error) switch
            {
                false => ErrorOperation(error),
                _ => await result.Content.ReadFromJsonAsync<Response>() ?? ErrorOperation("Failed to delete vehicle."),
            };
        }

        /// <summary>
        /// Deletes a vehicle brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle brand to delete.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>
        public async Task<Response> DeleteVehicleBrand(int id)
        {
            var result = await (await PrivateClient()).DeleteAsync($"{Constant.DeleteVehicleBrandRoute}/{id}");
            string? error = CheckResponseStatus(result);
            return string.IsNullOrEmpty(error) switch
            {
                false => ErrorOperation(error),
                _ => await result.Content.ReadFromJsonAsync<Response>() ?? ErrorOperation("Failed to delete vehicle brand."),
            };
        }

        /// <summary>
        /// Deletes a vehicle owner by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle owner to delete.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>
        public async Task<Response> DeleteVehicleOwner(int id)
        {
            var result = await (await PrivateClient()).DeleteAsync($"{Constant.DeleteVehicleOwnerRoute}/{id}");
            string? error = CheckResponseStatus(result);
            return string.IsNullOrEmpty(error) switch
            {
                false => ErrorOperation(error),
                _ => await result.Content.ReadFromJsonAsync<Response>() ?? ErrorOperation("Failed to delete vehicle owner."),
            };
        }

        // Get / Single
        /// <summary>
        /// Retrieves a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>A <see cref="GetVehicleResponseDTO"/> representing the vehicle details, or an empty object if not found.</returns>
        public async Task<GetVehicleResponseDTO> GetVehicle(int id)
        => await (await PrivateClient()).GetFromJsonAsync<GetVehicleResponseDTO>($"{Constant.GetVehicleRoute}/{id}") ?? new GetVehicleResponseDTO();

        /// <summary>
        /// Retrieves a vehicle brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle brand to retrieve.</param>
        /// <returns>A <see cref="GetVehicleBrandResponseDTO"/> representing the vehicle brand details, or an empty object if not found.</returns>
        public async Task<GetVehicleBrandResponseDTO> GetVehicleBrand(int id)
        => await (await PrivateClient()).GetFromJsonAsync<GetVehicleBrandResponseDTO>($"{Constant.GetVehicleBrandRoute}/{id}") ?? new GetVehicleBrandResponseDTO();

        /// <summary>
        /// Retrieves a vehicle owner by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle owner to retrieve.</param>
        /// <returns>A <see cref="GetVehicleOwnerResponseDTO"/> representing the vehicle owner details, or an empty object if not found.</returns>
        public async Task<GetVehicleOwnerResponseDTO> GetVehicleOwner(int id)
        => await (await PrivateClient()).GetFromJsonAsync<GetVehicleOwnerResponseDTO>($"{Constant.GetVehicleOwnerRoute}/{id}") ?? new GetVehicleOwnerResponseDTO();

        // Get / List
        /// <summary>
        /// Retrieves a list of all vehicles.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{GetVehicleResponseDTO}"/> containing all vehicle details.</returns>
        public async Task<IEnumerable<GetVehicleResponseDTO>> GetVehicles()
        {
            var result = await (await PrivateClient()).GetFromJsonAsync<IEnumerable<GetVehicleResponseDTO>>(Constant.GetVehiclesRoute);
            return result ?? Enumerable.Empty<GetVehicleResponseDTO>();
        }

        /// <summary>
        /// Retrieves a list of all vehicle brands.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{GetVehicleBrandResponseDTO}"/> containing all vehicle brand details.</returns>
        public async Task<IEnumerable<GetVehicleBrandResponseDTO>> GetVehicleBrands()
        => await (await PrivateClient()).GetFromJsonAsync<IEnumerable<GetVehicleBrandResponseDTO>>(Constant.GetVehicleBrandsRoute) ?? Enumerable.Empty<GetVehicleBrandResponseDTO>();

        /// <summary>
        /// Retrieves a list of all vehicle owners.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{GetVehicleOwnerResponseDTO}"/> containing all vehicle owner details.</returns>
        public async Task<IEnumerable<GetVehicleOwnerResponseDTO>> GetVehicleOwners()
        => await (await PrivateClient()).GetFromJsonAsync<IEnumerable<GetVehicleOwnerResponseDTO>>(Constant.GetVehicleOwnersRoute) ?? Enumerable.Empty<GetVehicleOwnerResponseDTO>();

        // Update
        /// <summary>
        /// Updates an existing vehicle.
        /// </summary>
        /// <param name="model">The updated vehicle details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the update operation.</returns>
        public async Task<Response> UpdateVehicle(UpdateVehicleRequestDTO model)
        {
            var result = await (await PrivateClient()).PutAsJsonAsync(Constant.UpdateVehicleRoute, model);
            string? error = CheckResponseStatus(result);
            return string.IsNullOrEmpty(error) switch
            {
                false => ErrorOperation(error),
                _ => await result.Content.ReadFromJsonAsync<Response>() ?? ErrorOperation("Failed to update vehicle."),
            };
        }

        /// <summary>
        /// Updates an existing vehicle brand.
        /// </summary>
        /// <param name="model">The updated vehicle brand details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the update operation.</returns>
        public async Task<Response> UpdateVehicleBrand(UpdateVehicleBrandRequestDTO model)
        {
            var result = await (await PrivateClient()).PutAsJsonAsync(Constant.UpdateVehicleBrandRoute, model);
            string? error = CheckResponseStatus(result);
            return string.IsNullOrEmpty(error) switch
            {
                false => ErrorOperation(error),
                _ => await result.Content.ReadFromJsonAsync<Response>() ?? ErrorOperation("Failed to update vehicle brand."),
            };
        }

        /// <summary>
        /// Updates an existing vehicle owner.
        /// </summary>
        /// <param name="model">The updated vehicle owner details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the update operation.</returns>
        public async Task<Response> UpdateVehicleOwner(UpdateVehicleOwnerRequestDTO model)
        {
            var result = await (await PrivateClient()).PutAsJsonAsync(Constant.UpdateVehicleOwnerRoute, model);
            string? error = CheckResponseStatus(result);
            return string.IsNullOrEmpty(error) switch
            {
                false => ErrorOperation(error),
                _ => await result.Content.ReadFromJsonAsync<Response>() ?? ErrorOperation("Failed to update vehicle owner."),
            };
        }
    }
}
