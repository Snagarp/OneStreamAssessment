using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request.Vehicles
{
    public class VehicleBaseDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty; // Initialize to avoid null reference issues

        [Required]
        public string Description { get; set; } = string.Empty; // Initialize to avoid null reference issues

        [Required]
        public int VehicleOwnerId { get; set; }

        [Range(1, 100, ErrorMessage = "Select Vehicle Brand")]
        public int VehicleBrandId { get; set; }
    }
}
