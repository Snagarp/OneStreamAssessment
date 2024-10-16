using Vehicle.Application.DTOs.Request.Account;
using Vehicle.Application.DTOs.Response;
using Vehicle.Application.DTOs.Response.Account;

namespace Vehicle.Application.Service
{
    /// <summary>
    /// Interface for account-related operations.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Creates an admin account in the system.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateAdmin();

        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="model">The account creation details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the account creation operation.</returns>
        Task<Response> CreateAccountAsync(CreateAccountDTO model);

        /// <summary>
        /// Logs in a user using the provided login details.
        /// </summary>
        /// <param name="model">The login details of the user.</param>
        /// <returns>A <see cref="LoginResponse"/> indicating the result of the login attempt.</returns>
        Task<LoginResponse> LoginAccountAsync(LoginDTO model);

        /// <summary>
        /// Refreshes the authentication token for a user.
        /// </summary>
        /// <param name="model">The refresh token details.</param>
        /// <returns>A <see cref="LoginResponse"/> indicating the result of the token refresh operation.</returns>
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model);

        /// <summary>
        /// Retrieves a list of all roles available in the system.
        /// </summary>
        /// <returns>A list of roles as <see cref="GetRoleDTO"/> objects.</returns>
        Task<IEnumerable<GetRoleDTO>> GetRolesAsync();

        /// <summary>
        /// Retrieves a list of users along with their assigned roles.
        /// </summary>
        /// <returns>A list of users with their roles as <see cref="GetUsersWithRolesResponseDTO"/> objects.</returns>
        Task<IEnumerable<GetUsersWithRolesResponseDTO>> GetUsersWithRolesAsync();

        /// <summary>
        /// Changes the role of a user.
        /// </summary>
        /// <param name="model">The details for changing the user's role.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the role change operation.</returns>
        Task<Response> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model);
    }
}
