using Moq;
using Vehicle.API.Controllers;
using Vehicle.Application.DTOs.Request.Account;
using Microsoft.AspNetCore.Mvc;
using Vehicle.Application.DTOs.Response;
using Vehicle.Application.Contracts;
using Vehicle.Application.DTOs.Response.Account;

namespace Vendor_Hub.Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccount> _mockAccountService;
        private readonly AccountController _accountController;

        public AccountControllerTests()
        {
            _mockAccountService = new Mock<IAccount>();
            _accountController = new AccountController(_mockAccountService.Object);
        }

        /// <summary>
        /// Tests that CreateAccount returns OkResult when given a valid model.
        /// </summary>
        [Fact]
        public async Task CreateAccount_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var createAccountDTO = new CreateAccountDTO();
            _mockAccountService.Setup(x => x.CreateAccountAsync(createAccountDTO)).ReturnsAsync(new Response { Flag = true });

            // Act
            var result = await _accountController.CreateAccount(createAccountDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        /// <summary>
        /// Tests that CreateAccount returns BadRequest when model state is invalid.
        /// </summary>
        [Fact]
        public async Task CreateAccount_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var createAccountDTO = new CreateAccountDTO(); // Invalid model
            _accountController.ModelState.AddModelError("EmailAddress", "Required");

            // Act
            var result = await _accountController.CreateAccount(createAccountDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Model cannot be null", badRequestResult.Value);
        }

        /// <summary>
        /// Tests that ChangeUserRole returns BadRequest when the role does not exist.
        /// </summary>
        [Fact]      
        public async Task ChangeUserRole_RoleNotFound_ReturnsBadRequest()
        {
            // Arrange
            var model = new ChangeUserRoleRequestDTO("user@example.com", "InvalidRole");
            var expectedResponse = new Response(false, "Role not found.");
            _mockAccountService.Setup(x => x.ChangeUserRoleAsync(model)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _accountController.ChangeUserRole(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Role not found.", badRequestResult.Value);
        }


        /// <summary>
        /// Tests that ChangeUserRole returns OkResult when given a valid model and role.
        /// </summary>
        [Fact]
        public async Task ChangeUserRole_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var model = new ChangeUserRoleRequestDTO("user@example.com", "Admin");
            var expectedResponse = new Response(true, "Role changed successfully.");
            _mockAccountService.Setup(x => x.ChangeUserRoleAsync(model)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _accountController.ChangeUserRole(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Change to result.Result
            var response = Assert.IsType<Response>(okResult.Value);
            Assert.True(response.Flag);
            Assert.Equal("Role changed successfully.", response.Message);
        }


        /// <summary>
        /// Tests that GetUserWithRoles returns OkResult with a list of users and roles.
        /// </summary>
        [Fact]
        public async Task GetUserWithRoles_ReturnsOkResultWithUsersAndRoles()
        {
            // Arrange
            var usersWithRoles = new List<GetUsersWithRolesResponseDTO>
    {
        new() { Name = "User1", Email = "user1@example.com", RoleId = "1", RoleName = "Admin" },
        new() { Name = "User2", Email = "user2@example.com", RoleId = "2", RoleName = "User" }
    };

            _mockAccountService.Setup(x => x.GetUsersWithRolesAsync()).ReturnsAsync(usersWithRoles);

            // Act
            var result = await _accountController.GetUserWithRoles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Change to result.Result
            var response = Assert.IsType<List<GetUsersWithRolesResponseDTO>>(okResult.Value);
            Assert.Equal(2, response.Count);
        }



        /// <summary>
        /// Tests that CreateAdmin returns OkResult.
        /// </summary>
        [Fact]
        public async Task CreateAdmin_ReturnsOkResult()
        {
            // Arrange
            _mockAccountService.Setup(x => x.CreateAdmin()).Returns(Task.CompletedTask);

            // Act
            var result = await _accountController.CreateAdmin();

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
