
using Application.DTO.Request.Account;
using Application.DTO.Response.Account;
using Application.DTO.Response;
namespace Application.Contracts
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
