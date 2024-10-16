using System.ComponentModel.DataAnnotations;

namespace Vehicle.Application.DTOs.Request.Vehicles
{
    public class CreateVehicleBrandRequestDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty; // Initialize to avoid null reference issues

        [Required]
        public string Location { get; set; } = string.Empty; // Initialize to avoid null reference issues
    }
}
