using Vehicle.Application.Contracts;
using Vehicle.Application.DTOs.Request.Account;
using Vehicle.Application.DTOs.Response;
using Vehicle.Application.DTOs.Response.Account;
using Microsoft.AspNetCore.Mvc;

namespace Vehicle.API.Controllers
{
    /// <summary>
    /// Controller for managing account-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccount account) : ControllerBase
    {
        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="model">The account creation details.</param>
        /// <returns>A response indicating the result of the account creation operation.</returns>
        [HttpPost("identity/create")]
        public async Task<ActionResult<Response>> CreateAccount(CreateAccountDTO model) => ModelState.IsValid switch
        {
            false => (ActionResult<Response>)BadRequest("Model cannot be null"),
            _ => (ActionResult<Response>)Ok(await account.CreateAccountAsync(model)),
        };

        /// <summary>
        /// Authenticates a user and generates a login token.
        /// </summary>
        /// <param name="model">The login details of the user.</param>
        /// <returns>A response indicating the result of the login operation.</returns>
        [HttpPost("identity/login")]
        public async Task<ActionResult<Response>> LoginAccount(LoginDTO model) => ModelState.IsValid switch
        {
            false => (ActionResult<Response>)BadRequest("Model cannot be null"),
            _ => (ActionResult<Response>)Ok(await account.LoginAccountAsync(model)),
        };

        /// <summary>
        /// Refreshes the authentication token for a user.
        /// </summary>
        /// <param name="model">The refresh token details.</param>
        /// <returns>A response indicating the result of the token refresh operation.</returns>
        [HttpPost("identity/refresh-token")]
        public async Task<ActionResult<Response>> RefreshToken(RefreshTokenDTO model) => ModelState.IsValid switch
        {
            false => (ActionResult<Response>)BadRequest("Model cannot be null"),
            _ => (ActionResult<Response>)Ok(await account.RefreshTokenAsync(model)),
        };

        /// <summary>
        /// Creates a new user role.
        /// </summary>
        /// <param name="model">The role creation details.</param>
        /// <returns>A response indicating the result of the role creation operation.</returns>
        [HttpPost("identity/role/create")]
        public async Task<ActionResult<Response>> CreateRole(CreateRoleDTO model) => ModelState.IsValid switch
        {
            false => (ActionResult<Response>)BadRequest("Model cannot be null"),
            _ => (ActionResult<Response>)Ok(await account.CreateRoleAsync(model)),
        };

        /// <summary>
        /// Retrieves a list of all user roles.
        /// </summary>
        /// <returns>A list of roles available in the system.</returns>
        [HttpGet("identity/role/list")]
        public async Task<ActionResult<IEnumerable<GetRoleDTO>>> GetRoles()
            => Ok(await account.GetRolesAsync());

        /// <summary>
        /// Creates an admin account.
        /// </summary>
        /// <returns>An Ok response indicating that the admin was created.</returns>
        [HttpPost("/setting")]
        public async Task<IActionResult> CreateAdmin()
        {
            await account.CreateAdmin();
            return Ok();
        }

        /// <summary>
        /// Retrieves a list of users along with their assigned roles.
        /// </summary>
        /// <returns>A list of users and their roles.</returns>
        [HttpGet("identity/users-with-roles")]
        public async Task<ActionResult<IEnumerable<GetUsersWithRolesResponseDTO>>> GetUserWithRoles()
            => Ok(await account.GetUsersWithRolesAsync());

        /// <summary>
        /// Changes the role of a user.
        /// </summary>
        /// <param name="model">The details for changing the user's role.</param>
        /// <returns>A response indicating the result of the role change operation.</returns>
        [HttpPost("identity/change-role")]
        public async Task<ActionResult<Response>> ChangeUserRole(ChangeUserRoleRequestDTO model)
        {
            var response = await account.ChangeUserRoleAsync(model);
            return response.Flag switch
            {
                false => (ActionResult<Response>)BadRequest(response.Message),// Return BadRequest if the role change fails
                _ => (ActionResult<Response>)Ok(response),
            };
        }

    }
}
