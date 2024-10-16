using Vehicle.Application.DTOs.Request.Account;
using Vehicle.Application.DTOs.Response;
using Vehicle.Application.DTOs.Response.Account;
namespace Vehicle.Application.Contracts
{
    public interface IAccount
    {
        Task CreateAdmin();
        Task<Response> CreateAccountAsync(CreateAccountDTO model);
        Task<LoginResponse> LoginAccountAsync(LoginDTO model);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model);
        Task<Response> CreateRoleAsync(CreateRoleDTO model);
        Task<IEnumerable<GetRoleDTO>> GetRolesAsync();
        Task<IEnumerable<GetUsersWithRolesResponseDTO>> GetUsersWithRolesAsync();
        Task<Response> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model);
    }
}
