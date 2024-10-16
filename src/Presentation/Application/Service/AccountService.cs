using Application.DTO.Request.Account;
using Application.DTO.Response;
using Application.DTO.Response.Account;
using Application.Extensions;
using System.Net.Http.Json;

namespace Application.Service
{
    /// <summary>
    /// Service for handling account-related operations such as login, registration, and role management.
    /// </summary>
    public class AccountService(HttpClientService httpClientService) : IAccountService
    {
        /// <summary>
        /// Logs in a user using the provided login details.
        /// </summary>
        /// <param name="model">The login details of the user.</param>
        /// <returns>A <see cref="LoginResponse"/> indicating the result of the login attempt.</returns>
        public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constant.LoginRoute, model);
                string? error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    return new LoginResponse(Flag: false, Message: error);

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!;
            }
            catch (Exception ex)
            {
                return new LoginResponse(Flag: false, Message: ex.Message);
            }
        }

        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="model">The account creation details.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the account creation operation.</returns>
        public async Task<Response> CreateAccountAsync(CreateAccountDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constant.RegisterRoute, model);
                string? error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    return new Response(Flag: false, Message: error);

                var result = await response.Content.ReadFromJsonAsync<Response>();
                return result!;
            }
            catch (Exception ex)
            {
                return new Response(Flag: false, Message: ex.Message);
            }
        }

        /// <summary>
        /// Checks the response status of an HTTP request.
        /// </summary>
        /// <param name="response">The HTTP response to check.</param>
        /// <returns>An error message if the response indicates a failure; otherwise, null.</returns>
        private static string? CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return $"Sorry, an unknown error occurred.{Environment.NewLine}Error Description:{Environment.NewLine}Status Code: {response.StatusCode}{Environment.NewLine}Reason Phrase: {response.ReasonPhrase}";
            else
                return null;
        }

        /// <summary>
        /// Creates an admin account.
        /// </summary>
        public async Task CreateAdmin()
        {
            try
            {
                var client = httpClientService.GetPublicClient();
                await client.PostAsync(Constant.CreateAdminRoute, null);
            }
            catch { }
        }

        /// <summary>
        /// Retrieves a list of all roles available in the system.
        /// </summary>
        /// <returns>A list of roles as <see cref="GetRoleDTO"/> objects.</returns>
        public async Task<IEnumerable<GetRoleDTO>> GetRolesAsync()
        {
            try
            {
                var privateClient = await httpClientService.GetPrivateClient();
                var response = await privateClient.GetAsync(Constant.GetRolesRoute);
                string? error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetRoleDTO>>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of users along with their assigned roles.
        /// </summary>
        /// <returns>A list of users with their roles as <see cref="GetUsersWithRolesResponseDTO"/> objects.</returns>
        public async Task<IEnumerable<GetUsersWithRolesResponseDTO>> GetUsersWithRolesAsync()
        {
            try
            {
                var privateClient = await httpClientService.GetPrivateClient();
                var response = await privateClient.GetAsync(Constant.GetUserWithRolesRoute);
                string? error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetUsersWithRolesResponseDTO>>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Changes the role of a user.
        /// </summary>
        /// <param name="model">The details for changing the user's role.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the role change operation.</returns>
        public async Task<Response> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model)
        {
            try
            {
                var publicClient = await httpClientService.GetPrivateClient();
                var response = await publicClient.PostAsJsonAsync(Constant.ChangeUserRoleRoute, model);
                string? error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    return new Response(false, error);

                var result = await response.Content.ReadFromJsonAsync<Response>();
                return result!;
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        /// <summary>
        /// Refreshes the authentication token for a user.
        /// </summary>
        /// <param name="model">The refresh token details.</param>
        /// <returns>A <see cref="LoginResponse"/> indicating the result of the token refresh operation.</returns>
        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model)
        {
            try
            {
                var publicClient = httpClientService.GetPublicClient();
                var response = await publicClient.PostAsJsonAsync(Constant.RefreshTokenRoute, model);
                string? error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    return new LoginResponse(false, error);

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!;
            }
            catch (Exception ex)
            {
                return new LoginResponse(false, ex.Message);
            }
        }
    }
}