using Vehicle.Application.DTOs.Request.Vehicles;
using Vehicle.Application.DTOs.Response;
using Vehicle.Application.DTOs.Response.Vehicles;
namespace Vehicle.Application.Contracts
{
    public interface IVehicle
    {
        Task<Response> AddVehicle(CreateVehicleRequestDTO model);
        Task<IEnumerable<GetVehicleResponseDTO>> GetVehicles();
        Task<GetVehicleResponseDTO?> GetVehicle(int id);
        Task<Response> DeleteVehicle(int id);
        Task<Response> UpdateVehicle(UpdateVehicleRequestDTO model);

        Task<Response> AddVehicleBrand(CreateVehicleBrandRequestDTO model);
        Task<IEnumerable<GetVehicleBrandResponseDTO>> GetVehicleBrands();
        Task<GetVehicleBrandResponseDTO> GetVehicleBrand(int id);
        Task<Response> DeleteVehicleBrand(int id);
        Task<Response> UpdateVehicleBrand(UpdateVehicleBrandRequestDTO model);

        Task<Response> AddVehicleOwner(CreateVehicleOwnerRequestDTO model);
        Task<IEnumerable<GetVehicleOwnerResponseDTO>> GetVehicleOwners();
        Task<GetVehicleOwnerResponseDTO> GetVehicleOwner(int id);
        Task<Response> DeleteVehicleOwner(int id);
        Task<Response> UpdateVehicleOwner(UpdateVehicleOwnerRequestDTO model);
    }
}
