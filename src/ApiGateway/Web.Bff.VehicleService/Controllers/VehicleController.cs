using Microsoft.AspNetCore.Mvc;
using Vehicle.Application.Contracts;
using Vehicle.Application.DTOs.Request.Vehicles;
using Vehicle.Application.DTOs.Response;
using Vehicle.Application.DTOs.Response.Vehicles;

namespace Web.Vehicle.HttpAggregator.Controllers
{
    /// <summary>
    /// Controller for managing vehicle-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicle _vehicle;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleController"/> class.
        /// </summary>
        /// <param name="vehicle">The vehicle service.</param>
        public VehicleController(IVehicle vehicle)
        {
            _vehicle = vehicle;
        }

        /// <summary>
        /// Creates a new vehicle.
        /// </summary>
        /// <param name="model">The details of the vehicle to be created.</param>
        /// <returns>A response indicating the result of the vehicle creation.</returns>
        [HttpPost("add-vehicle")]
        public async Task<ActionResult<Response>> Create(CreateVehicleRequestDTO model)
        {
            switch (ModelState.IsValid)
            {
                case false:
                    return BadRequest("Model cannot be null");
                default:
                {
                    var response = await _vehicle.AddVehicle(model);
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Creates a new vehicle brand.
        /// </summary>
        /// <param name="model">The details of the vehicle brand to be created.</param>
        /// <returns>A response indicating the result of the vehicle brand creation.</returns>
        [HttpPost("add-vehicle-brand")]
        public async Task<ActionResult<Response>> CreateBrand(CreateVehicleBrandRequestDTO model)
        {
            switch (ModelState.IsValid)
            {
                case false:
                    return BadRequest("Model cannot be null");
                default:
                {
                    var response = await _vehicle.AddVehicleBrand(model);
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Creates a new vehicle owner.
        /// </summary>
        /// <param name="model">The details of the vehicle owner to be created.</param>
        /// <returns>A response indicating the result of the vehicle owner creation.</returns>
        [HttpPost("add-vehicle-owner")]
        public async Task<ActionResult<Response>> CreateOwner(CreateVehicleOwnerRequestDTO model)
        {
            var response = await _vehicle.AddVehicleOwner(model);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a specific vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle.</param>
        /// <returns>The details of the specified vehicle, or a NotFound result if not found.</returns>
        [HttpGet("get-vehicle/{id}")]
        public async Task<ActionResult<GetVehicleResponseDTO>> Get(int id)
        {
            var vehicleResponse = await _vehicle.GetVehicle(id);
            return vehicleResponse switch
            {
                null => (ActionResult<GetVehicleResponseDTO>)NotFound(),// Return NotFound if vehicle is not found
                _ => (ActionResult<GetVehicleResponseDTO>)Ok(vehicleResponse),
            };
        }

        /// <summary>
        /// Retrieves a specific vehicle brand by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle brand.</param>
        /// <returns>The details of the specified vehicle brand, or NotFound if not found.</returns>
        [HttpGet("get-vehicle-brand/{id}")]
        public async Task<ActionResult<GetVehicleBrandResponseDTO>> GetBrand(int id)
        {
            var brandResponse = await _vehicle.GetVehicleBrand(id);
            return brandResponse switch
            {
                null => (ActionResult<GetVehicleBrandResponseDTO>)NotFound(),// Return NotFound if brand is not found
                _ => (ActionResult<GetVehicleBrandResponseDTO>)Ok(brandResponse),
            };
        }

        /// <summary>
        /// Retrieves a specific vehicle owner by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle owner.</param>
        /// <returns>The details of the specified vehicle owner, or NotFound if not found.</returns>
        [HttpGet("get-vehicle-owner/{id}")]
        public async Task<ActionResult<GetVehicleOwnerResponseDTO>> GetOwner(int id)
        {
            var ownerResponse = await _vehicle.GetVehicleOwner(id);
            return ownerResponse switch
            {
                null => (ActionResult<GetVehicleOwnerResponseDTO>)NotFound(),// Return NotFound if owner is not found
                _ => (ActionResult<GetVehicleOwnerResponseDTO>)Ok(ownerResponse),
            };
        }

        /// <summary>
        /// Retrieves a list of all vehicles.
        /// </summary>
        /// <returns>A list of vehicles available in the system.</returns>
        [HttpGet("get-vehicles")]
        public async Task<ActionResult<IEnumerable<GetVehicleResponseDTO>>> Gets() =>
            Ok(await _vehicle.GetVehicles());

        /// <summary>
        /// Retrieves a list of all vehicle brands.
        /// </summary>
        /// <returns>A list of vehicle brands available in the system.</returns>
        [HttpGet("get-vehicle-brands")]
        public async Task<ActionResult<IEnumerable<GetVehicleBrandResponseDTO>>> GetBrands() =>
            Ok(await _vehicle.GetVehicleBrands());

        /// <summary>
        /// Retrieves a list of all vehicle owners.
        /// </summary>
        /// <returns>A list of vehicle owners available in the system.</returns>
        [HttpGet("get-vehicle-owners")]
        public async Task<ActionResult<IEnumerable<GetVehicleOwnerResponseDTO>>> GetOwners() =>
            Ok(await _vehicle.GetVehicleOwners());

        /// <summary>
        /// Updates the details of a vehicle.
        /// </summary>
        /// <param name="model">The updated details of the vehicle.</param>
        /// <returns>A response indicating the result of the vehicle update.</returns>
        [HttpPut("update-vehicle")]
        public async Task<ActionResult<Response>> Update(UpdateVehicleRequestDTO model) =>
            Ok(await _vehicle.UpdateVehicle(model));

        /// <summary>
        /// Updates the details of a vehicle brand.
        /// </summary>
        /// <param name="model">The updated details of the vehicle brand.</param>
        /// <returns>A response indicating the result of the vehicle brand update.</returns>
        [HttpPut("update-vehicle-brand")]
        public async Task<ActionResult<Response>> UpdateBrand(UpdateVehicleBrandRequestDTO model) =>
            Ok(await _vehicle.UpdateVehicleBrand(model));

        /// <summary>
        /// Updates the details of a vehicle owner.
        /// </summary>
        /// <param name="model">The updated details of the vehicle owner.</param>
        /// <returns>A response indicating the result of the vehicle owner update.</returns>
        [HttpPut("update-vehicle-owner")]
        public async Task<ActionResult<Response>> UpdateOwner(UpdateVehicleOwnerRequestDTO model) =>
            Ok(await _vehicle.UpdateVehicleOwner(model));

        /// <summary>
        /// Deletes a specific vehicle by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to be deleted.</param>
        /// <returns>A response indicating the result of the vehicle deletion.</returns>
        [HttpDelete("delete-vehicle/{id}")]
        public async Task<ActionResult<Response>> Delete(int id)
        {
            var response = await _vehicle.DeleteVehicle(id);
            return response.Flag switch
            {
                false => (ActionResult<Response>)NotFound(response.Message),// Return NotFound if the vehicle is not found
                _ => (ActionResult<Response>)Ok(response),
            };
        }

        /// <summary>
        /// Deletes a specific vehicle brand by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle brand to be deleted.</param>
        /// <returns>A response indicating the result of the vehicle brand deletion.</returns>
        [HttpDelete("delete-vehicle-brand/{id}")]
        public async Task<ActionResult<Response>> DeleteBrand(int id)
        {
            var response = await _vehicle.DeleteVehicleBrand(id);
            return Ok(response);
        }

        /// <summary>
        /// Deletes a specific vehicle owner by ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle owner to be deleted.</param>
        /// <returns>A response indicating the result of the vehicle owner deletion.</returns>
        [HttpDelete("delete-vehicle-owner/{id}")]
        public async Task<ActionResult<Response>> DeleteOwner(int id)
        {
            var response = await _vehicle.DeleteVehicleOwner(id);
            return Ok(response);
        }
    }
}
