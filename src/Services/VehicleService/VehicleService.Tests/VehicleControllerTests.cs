using Moq;
using Vehicle.API.Controllers;
using Vehicle.Application.DTOs.Request.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Vehicle.Application.DTOs.Response;
using Vehicle.Application.DTOs.Response.Vehicles;
using Vehicle.Application.Contracts;

namespace Vendor_Hub.Tests
{
    public class VehicleControllerTests
    {
        private readonly Mock<IVehicle> _mockVehicleService;
        private readonly VehicleController _vehicleController;

        public VehicleControllerTests()
        {
            _mockVehicleService = new Mock<IVehicle>();
            _vehicleController = new VehicleController(_mockVehicleService.Object);
        }

        /// <summary>
        /// Tests that CreateVehicle returns an OkObjectResult when given a valid model.
        /// </summary>
        [Fact]
        public async Task CreateVehicle_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var createVehicleRequest = new CreateVehicleRequestDTO
            {
                Name = "Car",
                Description = "A fast car",
                VehicleOwnerId = 1,
                VehicleBrandId = 1
            };

            _mockVehicleService.Setup(x => x.AddVehicle(createVehicleRequest))
                .ReturnsAsync(new Response { Flag = true });

            // Act
            var result = await _vehicleController.Create(createVehicleRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<Response>(okResult.Value);
            Assert.True(response.Flag);
        }


        /// <summary>
        /// Tests that CreateVehicle returns a BadRequestObjectResult when given an invalid model.
        /// </summary>
        [Fact]
        public async Task CreateVehicle_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var createVehicleRequest = new CreateVehicleRequestDTO(); // Missing required fields
            _vehicleController.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _vehicleController.Create(createVehicleRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result); // Accessing Result for ActionResult
            Assert.Equal("Model cannot be null", badRequestResult.Value);
        }


        /// <summary>
        /// Tests that GetVehicle returns an OkObjectResult when given a valid vehicle ID.
        /// </summary>
        [Fact]
        public async Task GetVehicle_ValidId_ReturnsOkResult()
        {
            // Arrange
            int vehicleId = 1;
            var vehicleResponse = new GetVehicleResponseDTO { Id = vehicleId, Name = "Car", Description = "A fast car" };
            _mockVehicleService.Setup(x => x.GetVehicle(vehicleId)).ReturnsAsync(vehicleResponse);

            // Act
            var result = await _vehicleController.Get(vehicleId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<GetVehicleResponseDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<GetVehicleResponseDTO>(okResult.Value);
            Assert.Equal(vehicleId, response.Id);
        }


        /// <summary>
        /// Tests that GetVehicle returns a NotFoundResult when given an invalid vehicle ID.
        /// </summary>
        [Fact]
        public async Task GetVehicle_InvalidId_ReturnsNotFound()
        {
            // Arrange
            int vehicleId = 99; // Assume this ID does not exist
            _mockVehicleService.Setup(x => x.GetVehicle(vehicleId)).ReturnsAsync((GetVehicleResponseDTO?)null); // Cast to nullable type
            // Act
            var result = await _vehicleController.Get(vehicleId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        /// <summary>
        /// Tests that GetBrands returns an OkObjectResult containing a list of brands.
        /// </summary>
        [Fact]
        public async Task GetBrands_ReturnsOkResultWithListOfBrands()
        {
            // Arrange
            var brands = new List<GetVehicleBrandResponseDTO>
            {
                new()  { Id = 1, Name = "BrandA", Location = "LocationA" },
                new()  { Id = 2, Name = "BrandB", Location = "LocationB" }
            };
            _mockVehicleService.Setup(x => x.GetVehicleBrands()).ReturnsAsync(brands);

            // Act
            var result = await _vehicleController.GetBrands();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<GetVehicleBrandResponseDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            // Change the type assertion here
            var response = Assert.IsAssignableFrom<IEnumerable<GetVehicleBrandResponseDTO>>(okResult.Value);

            Assert.Equal(2, response.Count());
        }



        /// <summary>
        /// Tests that DeleteVehicle returns an OkObjectResult when given a valid vehicle ID.
        /// </summary>
        [Fact]
        public async Task DeleteVehicle_ValidId_ReturnsOkResult()
        {
            // Arrange
            int vehicleId = 1;
            _mockVehicleService.Setup(x => x.DeleteVehicle(vehicleId)).ReturnsAsync(new Response { Flag = true });

            // Act
            var result = await _vehicleController.Delete(vehicleId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result); // Change here
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<Response>(okResult.Value);
            Assert.True(response.Flag);
        }


        /// <summary>
        /// Tests that DeleteVehicle returns a NotFoundResult when given an invalid vehicle ID.
        /// </summary>
        [Fact]
        public async Task DeleteVehicle_InvalidId_ReturnsNotFound()
        {
            // Arrange
            int vehicleId = 99; // Assume this ID does not exist
            _mockVehicleService.Setup(x => x.DeleteVehicle(vehicleId)).ReturnsAsync(new Response { Flag = false, Message = "Vehicle not found." });

            // Act
            var result = await _vehicleController.Delete(vehicleId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result); // Change to check for NotFoundObjectResult
            var notFoundResult = result.Result as NotFoundObjectResult; // Cast to NotFoundObjectResult
            Assert.Equal("Vehicle not found.", notFoundResult?.Value); // Check the message
        }


        /// <summary>
        /// Tests that CreateBrand returns an OkObjectResult when given a valid model.
        /// </summary>
        [Fact]
        public async Task CreateBrand_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var createBrandDTO = new CreateVehicleBrandRequestDTO { Name = "BrandA", Location = "LocationA" };
            _mockVehicleService.Setup(x => x.AddVehicleBrand(createBrandDTO)).ReturnsAsync(new Response { Flag = true });

            // Act
            var result = await _vehicleController.CreateBrand(createBrandDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result); // Check for ActionResult<Response>
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result); // Check for OkObjectResult
            var response = Assert.IsType<Response>(okResult.Value); // Check for Response type
            Assert.True(response.Flag); // Verify the response indicates success
        }
    }
}
