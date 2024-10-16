using Application.Contracts;
using Application.DTO.Request.Vehicles;
using Application.DTO.Response;
using Application.DTO.Response.Vehicles;
using Domain.Entity.VehicleEntity;
using Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    /// <summary>
    /// Repository for handling operations related to vehicles, vehicle brands, and vehicle owners.
    /// </summary>
    internal class VehicleRepo(AppDbContext context): IVehicle
    {
        private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// Finds a vehicle by its name.
        /// </summary>
        /// <param name="name">The name of the vehicle.</param>
        /// <returns>The vehicle entity if found; otherwise, null.</returns>
        private async Task<Vehicle?> FindVehicleByName(string name) =>
            await _context.Vehicles.FirstOrDefaultAsync(x =>
                x.Name != null && StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, name) == 0);


        /// <summary>
        /// Finds a vehicle brand by its name.
        /// </summary>
        /// <param name="name">The name of the vehicle brand.</param>
        /// <returns>The vehicle brand entity if found; otherwise, null.</returns>
        private async Task<VehicleBrand?> FindVehicleBrandByName(string name) =>
            await _context.VehicleBrands.FirstOrDefaultAsync(x => 
                x.Name != null && StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, name) == 0);

        /// <summary>
        /// Finds a vehicle owner by their name.
        /// </summary>
        /// <param name="name">The name of the vehicle owner.</param>
        /// <returns>The vehicle owner entity if found; otherwise, null.</returns>
        private async Task<VehicleOwner?> FindVehicleOwnerByName(string name) =>
            await _context.VehicleOwners.FirstOrDefaultAsync(x => 
                x.Name != null && StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, name) == 0);

        /// <summary>
        /// Finds a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle.</param>
        /// <returns>The vehicle entity if found; otherwise, null.</returns>
        private async Task<Vehicle?> FindVehicleById(int id) =>
            await _context.Vehicles.Include(b => b.VehicleBrand).Include(o => o.VehicleOwner)
            .FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// Finds a vehicle brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle brand.</param>
        /// <returns>The vehicle brand entity if found; otherwise, null.</returns>
        private async Task<VehicleBrand?> FindVehicleBrandById(int id) =>
            await _context.VehicleBrands.FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// Finds a vehicle owner by their ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle owner.</param>
        /// <returns>The vehicle owner entity if found; otherwise, null.</returns>
        private async Task<VehicleOwner?> FindVehicleOwnerById(int id) =>
            await _context.VehicleOwners.FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// Saves changes asynchronously to the database.
        /// </summary>
        private async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        /// <summary>
        /// Creates a generic response with a failure message indicating a null result.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <returns>A <see cref="Response"/> indicating failure.</returns>
        private static Response NullResponse(string message) => new(false, message);

        /// <summary>
        /// Creates a response indicating that an entity already exists.
        /// </summary>
        /// <param name="message">The message indicating the entity already exists.</param>
        /// <returns>A <see cref="Response"/> indicating the entity already exists.</returns>
        private static Response AlreadyExistResponse(string message) => new(false, message);

        /// <summary>
        /// Creates a response indicating a successful operation.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <returns>A <see cref="Response"/> indicating success.</returns>
        private static Response OperationSuccessResponse(string message) => new(true, message);

        /// <summary>
        /// Adds a new vehicle to the database.
        /// </summary>
        /// <param name="model">The vehicle creation details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>
        public async Task<Response> AddVehicle(CreateVehicleRequestDTO model)
        {
            if (await FindVehicleByName(model.Name) != null) return AlreadyExistResponse("Vehicle already exists");
            _context.Vehicles.Add(model.Adapt<Vehicle>());
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle data saved");
        }

        /// <summary>
        /// Adds a new vehicle brand to the database.
        /// </summary>
        /// <param name="model">The vehicle brand creation details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>
        public async Task<Response> AddVehicleBrand(CreateVehicleBrandRequestDTO model)
        {
            if (await FindVehicleBrandByName(model.Name) != null) return AlreadyExistResponse("Vehicle Brand already exists");
            _context.VehicleBrands.Add(model.Adapt<VehicleBrand>());
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle Brand data saved");
        }

        /// <summary>
        /// Adds a new vehicle owner to the database.
        /// </summary>
        /// <param name="model">The vehicle owner creation details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>
        public async Task<Response> AddVehicleOwner(CreateVehicleOwnerRequestDTO model)
        {
            if (await FindVehicleOwnerByName(model.Name) != null) return AlreadyExistResponse("Vehicle Owner already exists");
            _context.VehicleOwners.Add(model.Adapt<VehicleOwner>());
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle Owner data saved");
        }

        /// <summary>
        /// Deletes a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the deletion.</returns>
        public async Task<Response> DeleteVehicle(int id)
        {
            var vehicle = await FindVehicleById(id);
            if (vehicle == null) return NullResponse("Vehicle not found");
            _context.Vehicles.Remove(vehicle);
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle deleted");
        }

        /// <summary>
        /// Deletes a vehicle brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle brand to delete.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the deletion.</returns>
        public async Task<Response> DeleteVehicleBrand(int id)
        {
            var vehicleBrand = await FindVehicleBrandById(id);
            if (vehicleBrand == null) return NullResponse("Vehicle Brand not found");
            _context.VehicleBrands.Remove(vehicleBrand);
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle Brand deleted");
        }

        /// <summary>
        /// Deletes a vehicle owner by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle owner to delete.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the deletion.</returns>
        public async Task<Response> DeleteVehicleOwner(int id)
        {
            var vehicleOwner = await FindVehicleOwnerById(id);
            if (vehicleOwner == null) return NullResponse("Vehicle Owner not found");
            _context.VehicleOwners.Remove(vehicleOwner);
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle Owner deleted");
        }

        /// <summary>
        /// Retrieves a vehicle by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle to retrieve.</param>
        /// <returns>A <see cref="GetVehicleResponseDTO"/> representing the vehicle details, or null if not found.</returns>
        public async Task<GetVehicleResponseDTO?> GetVehicle(int id)
        {
            var vehicle = await FindVehicleById(id);
            return vehicle?.Adapt<GetVehicleResponseDTO>();
        }

        /// <summary>
        /// Retrieves a vehicle brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle brand to retrieve.</param>
        /// <returns>A <see cref="GetVehicleBrandResponseDTO"/> representing the vehicle brand details, or null if not found.</returns>
        public async Task<GetVehicleBrandResponseDTO> GetVehicleBrand(int id)
        {
            var vehicleBrand = await FindVehicleBrandById(id);
            return vehicleBrand?.Adapt<GetVehicleBrandResponseDTO>() ?? new GetVehicleBrandResponseDTO();
        }


        /// <summary>
        /// Retrieves a vehicle owner by its ID.
        /// </summary>
        /// <param name="id">The ID of the vehicle owner to retrieve.</param>
        /// <returns>A <see cref="GetVehicleOwnerResponseDTO"/> representing the vehicle owner details, or null if not found.</returns>
        public async Task<GetVehicleOwnerResponseDTO> GetVehicleOwner(int id)
        {
            var vehicleOwner = await FindVehicleOwnerById(id);
            return vehicleOwner?.Adapt<GetVehicleOwnerResponseDTO>() ?? new GetVehicleOwnerResponseDTO();
        }

        /// <summary>
        /// Retrieves a list of all vehicles.
        /// </summary>
        /// <returns>A list of all vehicles as <see cref="GetVehicleResponseDTO"/> objects.</returns>
        public async Task<IEnumerable<GetVehicleResponseDTO>> GetVehicles()
        {
            var data = await _context.Vehicles
                .Include(b => b.VehicleBrand)
                .Include(o => o.VehicleOwner)
                .ToListAsync();

            return data.Select(vehicle => new GetVehicleResponseDTO
            {
                Id = vehicle.Id,
                Name = vehicle.Name ?? string.Empty,
                Description = vehicle.Description ?? string.Empty,
                VehicleOwnerId = vehicle.VehicleOwnerId,
                VehicleBrandId = vehicle.VehicleBrandId,
                VehicleBrand = new GetVehicleBrandResponseDTO
                {
                    Id = vehicle.VehicleBrand?.Id ?? 0, // Default to 0 if VehicleBrand is null
                    Name = vehicle.VehicleBrand?.Name ?? string.Empty,
                    Location = vehicle.VehicleBrand?.Location ?? string.Empty
                },
                VehicleOwner = new GetVehicleOwnerResponseDTO
                {
                    Id = vehicle.VehicleOwner?.Id ?? 0, // Default to 0 if VehicleOwner is null
                    Name = vehicle.VehicleOwner?.Name ?? string.Empty,
                    Address = vehicle.VehicleOwner?.Address ?? string.Empty
                }
            }).ToList();
        }


        /// <summary>
        /// Retrieves a list of all vehicle brands.
        /// </summary>
        /// <returns>A list of vehicle brands as <see cref="GetVehicleBrandResponseDTO"/> objects.</returns>
        public async Task<IEnumerable<GetVehicleBrandResponseDTO>> GetVehicleBrands() =>
            (await _context.VehicleBrands.ToListAsync()).Adapt<List<GetVehicleBrandResponseDTO>>();

        /// <summary>
        /// Retrieves a list of all vehicle owners.
        /// </summary>
        /// <returns>A list of vehicle owners as <see cref="GetVehicleOwnerResponseDTO"/> objects.</returns>
        public async Task<IEnumerable<GetVehicleOwnerResponseDTO>> GetVehicleOwners() =>
            (await _context.VehicleOwners.ToListAsync()).Adapt<List<GetVehicleOwnerResponseDTO>>();

        /// <summary>
        /// Updates an existing vehicle's details.
        /// </summary>
        /// <param name="model">The updated vehicle details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the update operation.</returns>
        public async Task<Response> UpdateVehicle(UpdateVehicleRequestDTO model)
        {
            var vehicle = await FindVehicleById(model.Id);
            if (vehicle == null) return NullResponse("Vehicle not found");
            _context.Entry(vehicle).State = EntityState.Detached;
            _context.Vehicles.Update(model.Adapt<Vehicle>());
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle data updated");
        }

        /// <summary>
        /// Updates an existing vehicle brand's details.
        /// </summary>
        /// <param name="model">The updated vehicle brand details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the update operation.</returns>
        public async Task<Response> UpdateVehicleBrand(UpdateVehicleBrandRequestDTO model)
        {
            var vehicleBrand = await FindVehicleBrandById(model.Id);
            if (vehicleBrand == null) return NullResponse("Vehicle Brand not found");
            _context.Entry(vehicleBrand).State = EntityState.Detached;
            _context.VehicleBrands.Update(model.Adapt<VehicleBrand>());
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle Brand data updated");
        }

        /// <summary>
        /// Updates an existing vehicle owner's details.
        /// </summary>
        /// <param name="model">The updated vehicle owner details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the update operation.</returns>
        public async Task<Response> UpdateVehicleOwner(UpdateVehicleOwnerRequestDTO model)
        {
            var vehicleOwner = await FindVehicleOwnerById(model.Id);
            if (vehicleOwner == null) return NullResponse("Vehicle Owner not found");
            _context.Entry(vehicleOwner).State = EntityState.Detached;
            _context.VehicleOwners.Update(model.Adapt<VehicleOwner>());
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle Owner data updated");
        }

    }
}
