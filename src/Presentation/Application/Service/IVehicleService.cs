using Application.DTO.Request.Vehicles;
using Application.DTO.Response;
using Application.DTO.Response.Vehicles;

namespace Application.Service
{
    /// <summary>
    /// Interface for vehicle-related operations, including vehicles, brands, and owners.
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Adds a new vehicle to the system.
        /// </summary>
        /// <param name="model">The vehicle creation details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the vehicle addition.</returns>
        Task<Response> AddVehicle(CreateVehicleRequestDTO model);

        /// <summary>
        /// Retrieves a list of all vehicles.
        /// </summary>
        /// <returns>A list of vehicles as <see cref="GetVehicleResponseDTO"/> objects.</returns>
        Task<IEnumerable<GetVehicleResponseDTO>> GetVehicles();

        /// <summary>
        /// Retrieves a specific vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>A <see cref="GetVehicleResponseDTO"/> representing the vehicle details.</returns>
        Task<GetVehicleResponseDTO> GetVehicle(int id);

        /// <summary>
        /// Deletes a specific vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the vehicle deletion.</returns>
        Task<Response> DeleteVehicle(int id);

        /// <summary>
        /// Updates an existing vehicle's details.
        /// </summary>
        /// <param name="model">The updated vehicle details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the update operation.</returns>
        Task<Response> UpdateVehicle(UpdateVehicleRequestDTO model);

        /// <summary>
        /// Adds a new vehicle brand to the system.
        /// </summary>
        /// <param name="model">The vehicle brand creation details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the brand addition.</returns>
        Task<Response> AddVehicleBrand(CreateVehicleBrandRequestDTO model);

        /// <summary>
        /// Retrieves a list of all vehicle brands.
        /// </summary>
        /// <returns>A list of vehicle brands as <see cref="GetVehicleBrandResponseDTO"/> objects.</returns>
        Task<IEnumerable<GetVehicleBrandResponseDTO>> GetVehicleBrands();

        /// <summary>
        /// Retrieves a specific vehicle brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle brand to retrieve.</param>
        /// <returns>A <see cref="GetVehicleBrandResponseDTO"/> representing the brand details.</returns>
        Task<GetVehicleBrandResponseDTO> GetVehicleBrand(int id);

        /// <summary>
        /// Deletes a specific vehicle brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle brand to delete.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the brand deletion.</returns>
        Task<Response> DeleteVehicleBrand(int id);

        /// <summary>
        /// Updates an existing vehicle brand's details.
        /// </summary>
        /// <param name="model">The updated vehicle brand details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the update operation.</returns>
        Task<Response> UpdateVehicleBrand(UpdateVehicleBrandRequestDTO model);

        /// <summary>
        /// Adds a new vehicle owner to the system.
        /// </summary>
        /// <param name="model">The vehicle owner creation details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the owner addition.</returns>
        Task<Response> AddVehicleOwner(CreateVehicleOwnerRequestDTO model);

        /// <summary>
        /// Retrieves a list of all vehicle owners.
        /// </summary>
        /// <returns>A list of vehicle owners as <see cref="GetVehicleOwnerResponseDTO"/> objects.</returns>
        Task<IEnumerable<GetVehicleOwnerResponseDTO>> GetVehicleOwners();

        /// <summary>
        /// Retrieves a specific vehicle owner by their ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle owner to retrieve.</param>
        /// <returns>A <see cref="GetVehicleOwnerResponseDTO"/> representing the owner details.</returns>
        Task<GetVehicleOwnerResponseDTO> GetVehicleOwner(int id);

        /// <summary>
        /// Deletes a specific vehicle owner by their ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle owner to delete.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the owner deletion.</returns>
        Task<Response> DeleteVehicleOwner(int id);

        /// <summary>
        /// Updates an existing vehicle owner's details.
        /// </summary>
        /// <param name="model">The updated vehicle owner details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the update operation.</returns>
        Task<Response> UpdateVehicleOwner(UpdateVehicleOwnerRequestDTO model);
    }
}
