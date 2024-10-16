#pragma warning disable IDE0301

using Vehicle.Application.Contracts;
using Vehicle.Application.DTOs.Request.Account;
using Vehicle.Application.DTOs.Response;
using Vehicle.Application.DTOs.Response.Account;
using Vehicle.Application.Extensions;
using Vehicle.Domain.Entity.Authentication;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Vehicle.Infrastructure.Data;
namespace Vehicle.Infrastructure.Repository
{
    public class AccountRepository
       (RoleManager<IdentityRole> roleManager,
       UserManager<ApplicationUser> userManager, IConfiguration config,
       SignInManager<ApplicationUser> signInManager, AppDbContext context) : IAccount
    {
        /// <summary>
        /// Finds a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to find.</param>
        /// <returns>An <see cref="ApplicationUser"/> if found; otherwise, <c>null</c>.</returns>
        private async Task<ApplicationUser?> FindUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
            }

            return await userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Finds a role by its name.
        /// </summary>
        /// <param name="roleName">The name of the role to find.</param>
        /// <returns>An <see cref="IdentityRole"/> if found; otherwise, <c>null</c>.</returns>
        private async Task<IdentityRole?> FindRoleByNameAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName), "Role name cannot be null or empty.");
            }

            return await roleManager.FindByNameAsync(roleName);
        }

        /// <summary>
        /// Generates a cryptographically secure random refresh token.
        /// </summary>
        /// <returns>A base64-encoded string representing the refresh token.</returns>
        private static string GenerateRefreshToken() =>
            // Generate a random byte array of size 64 and convert it to a base64-encoded string
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));


        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified user.
        /// </summary>
        /// <param name="user">The <see cref="ApplicationUser"/> for whom the token is being generated.</param>
        /// <returns>A JWT as a string if successful; otherwise, an empty string if an error occurs.</returns>
        private async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {
                switch (user)
                {
                    case null:
                        throw new ArgumentNullException(nameof(user), "User cannot be null");
                }

                // Retrieve the JWT key from configuration
                var key = config["Jwt:Key"];
                if (string.IsNullOrEmpty(key))
                {
                    throw new InvalidOperationException("JWT Key is not configured properly.");
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // Get user roles and handle potential nulls
                var roles = await userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "User"; // Default to "User" if no roles are found

                var userClaims = new[]
                {
            new Claim(ClaimTypes.Name, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, role),
            new Claim("Fullname", user.Name ?? string.Empty)
        };

                // Create the token with the claims and signing credentials
                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes (you can use a logging library or framework)
                Console.WriteLine($"Error generating token: {ex.Message}");
                return string.Empty; // Return an empty string to indicate a failure in token generation
            }
        }

        /// <summary>
        /// Assigns a user to a specified role.
        /// </summary>
        /// <param name="user">The <see cref="ApplicationUser"/> to be assigned to the role.</param>
        /// <param name="role">The <see cref="IdentityRole"/> to assign to the user.</param>
        /// <returns>A <see cref="Response"/> indicating the result of the operation.</returns>
        private async Task<Response> AssignUserToRole(ApplicationUser user, IdentityRole role)
        {
            // Validate that the user and role are not null
            if (user == null || role == null || role.Name == null)
            {
                return new Response(false, "User or role cannot be null.");
            }

            // Check if the role exists, and create it if it doesn't
            var existingRole = await FindRoleByNameAsync(role.Name);
            switch (existingRole)
            {
                case null:
                {
                    var createRoleResult = await CreateRoleAsync(role.Adapt(new CreateRoleDTO()));
                    switch (createRoleResult.Flag)
                    {
                        case false:
                            return new Response(false, "Failed to create the role.");
                    }

                    break;
                }
            }

            // Assign the user to the role
            IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);
            string error = CheckResponse(result);
            return string.IsNullOrEmpty(error) switch
            {
                false => new Response(false, error),
                _ => new Response(true, $"{user.Name} successfully assigned to the {role.Name} role."),
            };
        }

        /// <summary>
        /// Checks the result of an Identity operation and returns any error messages if the operation failed.
        /// </summary>
        /// <param name="result">The <see cref="IdentityResult"/> from an Identity operation.</param>
        /// <returns>A string containing error messages if the operation failed; otherwise, an empty string.</returns>
        private static string CheckResponse(IdentityResult result)
        {
            switch (result.Succeeded)
            {
                case false:
                {
                    var errors = result.Errors.Select(error => error.Description);
                    return string.Join(Environment.NewLine, errors);
                }
                default:
                    return string.Empty; // Return an empty string instead of null to avoid null reference issues
            }
        }

        /// <summary>
        /// Changes the role of a user to a new role.
        /// </summary>
        /// <param name="model">The <see cref="ChangeUserRoleRequestDTO"/> containing the user's email and the new role name.</param>
        /// <returns>A <see cref="Response"/> indicating whether the operation was successful or not.</returns>
        public async Task<Response> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model)
        {
            var role = await FindRoleByNameAsync(model.RoleName);
            switch (role)
            {
                case null:
                    return new Response(false, "Role not found");
            }

            var user = await FindUserByEmailAsync(model.UserEmail);
            switch (user)
            {
                case null:
                    return new Response(false, "User not found");
            }

            var previousRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            switch (string.IsNullOrEmpty(previousRole))
            {
                case false:
                {
                    var removeOldRole = await userManager.RemoveFromRoleAsync(user, previousRole);
                    var error = CheckResponse(removeOldRole);
                    switch (string.IsNullOrEmpty(error))
                    {
                        case false:
                            return new Response(false, error);
                    }
                    break;
                }
            }

            var result = await userManager.AddToRoleAsync(user, model.RoleName);
            var response = CheckResponse(result);
            return string.IsNullOrEmpty(response) switch
            {
                false => new Response(false, response),
                _ => new Response(true, "Role changed successfully."),
            };
        }

        /// <summary>
        /// Creates a new user account with the specified details.
        /// </summary>
        /// <param name="model">The <see cref="CreateAccountDTO"/> containing the account details.</param>
        /// <returns>A <see cref="Response"/> indicating whether the account creation was successful or not.</returns>
        public async Task<Response> CreateAccountAsync(CreateAccountDTO model)
        {
            try
            {
                if (await FindUserByEmailAsync(model.EmailAddress) != null)
                    return new Response(false, "Sorry, user already exists.");

                var user = new ApplicationUser
                {
                    Name = model.Name,
                    UserName = model.EmailAddress,
                    Email = model.EmailAddress
                };

                var result = await userManager.CreateAsync(user, model.Password);
                string error = CheckResponse(result);
                switch (string.IsNullOrEmpty(error))
                {
                    case false:
                        return new Response(false, error);
                    default:
                    {
                        var roleAssignmentResult = await AssignUserToRole(user, new IdentityRole { Name = model.Role });
                        return new Response(roleAssignmentResult.Flag, roleAssignmentResult.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return new Response(false, $"Error occurred while creating the account: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates an admin account if it does not already exist.
        /// </summary>
        public async Task CreateAdmin()
        {
            try
            {
                if (await FindRoleByNameAsync(Constant.Role.Admin) != null) return;

                var admin = new CreateAccountDTO
                {
                    Name = "Admin",
                    Password = "Admin@123",
                    EmailAddress = "admin@admin.com",
                    Role = Constant.Role.Admin
                };

                await CreateAccountAsync(admin);
            }
            catch (Exception ex)
            {
                // Log the exception if needed, using a logging framework or mechanism
                Console.WriteLine($"Error creating admin account: {ex.Message}");
            }
        }


        /// <summary>
        /// Creates a new role in the system if it does not already exist.
        /// </summary>
        /// <param name="model">The <see cref="CreateRoleDTO"/> containing the role details to be created.</param>
        /// <returns>A <see cref="Response"/> indicating whether the role creation was successful or not.</returns>
        public async Task<Response> CreateRoleAsync(CreateRoleDTO model)
        {
            try
            {
                // Validate that the role name is not null or empty
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    return new Response(false, "Role name cannot be null or empty.");
                }

                // Check if the role already exists
                if (await FindRoleByNameAsync(model.Name) != null)
                {
                    return new Response(false, $"{model.Name} already exists.");
                }

                // Attempt to create the role
                var result = await roleManager.CreateAsync(new IdentityRole(model.Name));
                var error = CheckResponse(result);

                return string.IsNullOrEmpty(error) switch
                {
                    false => new Response(false, error),
                    _ => new Response(true, $"{model.Name} role successfully created."),
                };
            }
            catch (Exception ex)
            {
                // Log the error and rethrow it to handle at a higher level or provide debugging info
                Console.WriteLine($"Error creating role: {ex.Message}");
                return new Response(false, $"An error occurred while creating the role: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves all roles in the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{GetRoleDTO}"/> containing the list of roles, or an empty list if no roles are found.</returns>
        public async Task<IEnumerable<GetRoleDTO>> GetRolesAsync()
        {
            try
            {
                var roles = await roleManager.Roles.ToListAsync();
                return roles.Adapt<IEnumerable<GetRoleDTO>>() ?? [];
            }
            catch (Exception ex)
            {
                // Log the error for debugging or tracking purposes
                Console.WriteLine($"Error retrieving roles: {ex.Message}");
                return []; // Return an empty list if there's an error
            }
        }

        /// <summary>
        /// Retrieves a list of users along with their assigned roles.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{GetUsersWithRolesResponseDTO}"/> containing user details and their roles.
        /// If an error occurs, an empty list is returned.
        /// </returns>
        public async Task<IEnumerable<GetUsersWithRolesResponseDTO>> GetUsersWithRolesAsync()
        {
            try
            {
                var allUsers = await userManager.Users.ToListAsync();
                if (allUsers == null || allUsers.Count == 0)
                    return Enumerable.Empty<GetUsersWithRolesResponseDTO>();

                // Use target-typed new expression for initialization
                var userList = new List<GetUsersWithRolesResponseDTO>();

                // Fetch roles once
                var roles = await roleManager.Roles.ToListAsync();

                foreach (var user in allUsers)
                {
                    var userRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();

                    // Find the role in a case-insensitive manner
                    var roleInfo = roles.FirstOrDefault(r =>
                        r.Name != null && r.Name.Equals(userRole, StringComparison.OrdinalIgnoreCase));

                    if (roleInfo != null)
                    {
                        userList.Add(new()
                        {
                            Name = user.Name,
                            Email = user.Email,
                            RoleId = roleInfo.Id,
                            RoleName = roleInfo.Name
                        });
                    }
                }

                return userList;
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine($"Error retrieving users with roles: {ex.Message}");
                return Enumerable.Empty<GetUsersWithRolesResponseDTO>();
            }
        }



        /// <summary>
        /// Authenticates a user based on their email and password and generates JWT and refresh tokens if successful.
        /// </summary>
        /// <param name="model">The <see cref="LoginDTO"/> containing the user's login details.</param>
        /// <returns>A <see cref="LoginResponse"/> indicating the result of the login attempt.</returns>
        public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
        {
            try
            {
                var user = await FindUserByEmailAsync(model.EmailAddress);
                switch (user)
                {
                    case null:
                        return new LoginResponse(false, "User not found");
                }

                SignInResult result;
                try
                {
                    result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                }
                catch (Exception)
                {
                    return new LoginResponse(false, "Invalid credentials");
                }

                switch (result.Succeeded)
                {
                    case false:
                        return new LoginResponse(false, "Invalid credentials");
                }

                string jwtToken = await GenerateToken(user);
                string refreshToken = GenerateRefreshToken();

                if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return new LoginResponse(false, "An error occurred while logging in, please contact administration.");
                }

                var saveResult = await SaveRefreshToken(user.Id, refreshToken);
                return saveResult.Flag switch
                {
                    true => new LoginResponse(true, $"{user.Name} successfully logged in", jwtToken, refreshToken),
                    _ => new LoginResponse(false, "Failed to save refresh token, please try again."),
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse(false, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Refreshes the user's authentication tokens using the provided refresh token.
        /// </summary>
        /// <param name="model">The <see cref="RefreshTokenDTO"/> containing the current refresh token.</param>
        /// <returns>A <see cref="LoginResponse"/> with new JWT and refresh tokens if successful.</returns>
        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model)
        {
            try
            {
                var token = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == model.Token);
                if (token == null || token.UserID == null) return new LoginResponse(false, "Invalid refresh token.");

                var user = await userManager.FindByIdAsync(token.UserID);
                switch (user)
                {
                    case null:
                        return new LoginResponse(false, "User not found.");
                }

                string newToken = await GenerateToken(user);
                string newRefreshToken = GenerateRefreshToken();

                var saveResult = await SaveRefreshToken(user.Id, newRefreshToken);
                return saveResult.Flag switch
                {
                    true => new LoginResponse(true, $"{user.Name} successfully re-logged in", newToken, newRefreshToken),
                    _ => new LoginResponse(false, "Failed to save new refresh token."),
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse(false, $"An error occurred: {ex.Message}");
            }
        }


        /// <summary>
        /// Saves or updates the refresh token for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom the refresh token is being saved.</param>
        /// <param name="token">The refresh token to be saved.</param>
        /// <returns>A <see cref="Response"/> indicating whether the operation was successful or not.</returns>
        private async Task<Response> SaveRefreshToken(string userId, string token)
        {
            try
            {
                var existingToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.UserID == userId);
                switch (existingToken)
                {
                    case null:
                        context.RefreshTokens.Add(new RefreshToken { UserID = userId, Token = token });
                        break;
                    default:
                        existingToken.Token = token;
                        break;
                }

                await context.SaveChangesAsync();
                return new Response(true, "Refresh token saved successfully.");
            }
            catch (Exception ex)
            {
                return new Response(false, $"An error occurred while saving the refresh token: {ex.Message}");
            }
        }

    }
}
