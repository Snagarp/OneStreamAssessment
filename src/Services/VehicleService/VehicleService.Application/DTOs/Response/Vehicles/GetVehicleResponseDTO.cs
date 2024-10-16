using Vehicle.Application.DTOs.Request.Vehicles;

namespace Vehicle.Application.DTOs.Response.Vehicles
{
    /// <summary>
    /// Represents the response data transfer object for a vehicle, including its brand and owner details.
    /// </summary>
    public class GetVehicleResponseDTO : VehicleBaseDTO
    {
        /// <summary>
        /// Gets or sets the ID of the vehicle.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the vehicle brand details.
        /// </summary>
        public virtual GetVehicleBrandResponseDTO? VehicleBrand { get; set; } = new GetVehicleBrandResponseDTO();

        /// <summary>
        /// Gets or sets the vehicle owner details.
        /// </summary>
        public virtual GetVehicleOwnerResponseDTO? VehicleOwner { get; set; } = new GetVehicleOwnerResponseDTO();
    }
}
