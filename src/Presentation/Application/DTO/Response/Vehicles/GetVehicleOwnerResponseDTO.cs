using Application.DTO.Request.Vehicles;

namespace Application.DTO.Response.Vehicles
{
    public class GetVehicleOwnerResponseDTO : CreateVehicleOwnerRequestDTO
    {
        public int Id { get; set; }
#pragma warning disable IDE0028
        public virtual ICollection<GetVehicleResponseDTO> Vehicles { get; set; } = new List<GetVehicleResponseDTO>(); // Initialized to avoid null reference
#pragma warning restore IDE0028
    }
}
