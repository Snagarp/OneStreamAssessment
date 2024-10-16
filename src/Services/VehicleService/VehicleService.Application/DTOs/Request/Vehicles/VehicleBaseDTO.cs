using System.ComponentModel.DataAnnotations;

namespace Vehicle.Application.DTOs.Request.Vehicles
{
    public class VehicleBaseDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty; 

        [Required]
        public string Description { get; set; } = string.Empty; 

        [Required]
        public int VehicleOwnerId { get; set; }

        [Range(1, 100, ErrorMessage = "Select Vehicle Brand")]
        public int VehicleBrandId { get; set; }
    }
}
