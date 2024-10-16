using Application.DTO.Request.Vehicles;

namespace Application.DTO.Response.Vehicles
{
    /// <summary>
    /// Represents the response data for a vehicle brand and its associated vehicles.
    /// </summary>
    public class GetVehicleBrandResponseDTO : CreateVehicleBrandRequestDTO
    {
        /// <summary>
        /// The unique identifier of the vehicle brand.
        /// </summary>
        public int Id { get; set; }

#pragma warning disable IDE0028
        /// <summary>
        /// The vehicles associated with this brand.
        /// </summary>
        public virtual ICollection<GetVehicleResponseDTO> Vehicles { get; set; } = new List<GetVehicleResponseDTO>();
#pragma warning restore IDE0028
    }
}
