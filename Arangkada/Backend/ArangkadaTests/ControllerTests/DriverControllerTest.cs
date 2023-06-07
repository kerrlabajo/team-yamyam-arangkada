using ArangkadaAPI.Controllers;
using ArangkadaAPI.Dtos.Driver;
using ArangkadaAPI.Dtos.Vehicle;
using ArangkadaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArangkadaTests.ControllerTests
{
    public class DriverControllerTest
    {
        private readonly DriversController _controller;
        private readonly Mock<IDriverService> _fakeDriverService;
        private readonly Mock<IVehicleService> _fakeVehicleService;
        private readonly Mock<ILogger<DriversController>> _logger;
        public DriverControllerTest()
        {
            _fakeDriverService = new Mock<IDriverService>();
            _fakeVehicleService = new Mock<IVehicleService>();
            _logger = new Mock<ILogger<DriversController>>();

            _controller = new DriversController(_fakeDriverService.Object, _fakeVehicleService.Object, _logger.Object);

        }

        //CreateDriver(201)
        [Fact]
        public async void AddDriver_NoExistingDriver_ReturnsCreatedDriver()
        {
            // Arrange
            var newDriver = new DriverCreationDto
            {
                OperatorName = "Operator 1",
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1",
                Category = "Primary"
            };

            _fakeDriverService.Setup(x => x.CreateDriver(newDriver))
                              .ReturnsAsync(new DriverDto());

            // Act
            var result = await _controller.AddDriver(newDriver);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        //CreateDriver(409)
        [Fact]
        public async void AddDriver_HasExistingDriver_ReturnsConflict()
        {
            // Arrange
            var newDriver = new DriverCreationDto
            {
                OperatorName = "Operator 1",
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1",
                Category = "Primary"
            };

            var existingDriver = new DriverDto
            {
                Id = 1,
                OperatorName = "Operator 1",
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1",
                Category = "Primary"
            };

            _fakeDriverService.Setup(x => x.GetByFullName(newDriver.FullName))
                              .ReturnsAsync(existingDriver);

            // Act
            var result = await _controller.AddDriver(newDriver);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }


        //CreateDriver(400)
        [Fact]
        public async void AddDriver_InvalidDriverBody_ReturnsBadRequest()
        {
            // Arrange
            var newDriver = new DriverCreationDto
            {
                OperatorName = "Operator 1",
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1",
                Category = "Primary"
            };
            _controller.ModelState.AddModelError("OperatorName", "Required");
            _controller.ModelState.AddModelError("FullName", "Required");
            _controller.ModelState.AddModelError("Address", "Required");
            _controller.ModelState.AddModelError("ContactNumber", "Required");
            _controller.ModelState.AddModelError("LicenseNumber", "Required");
            _controller.ModelState.AddModelError("ExpirationDate", "Required");
            _controller.ModelState.AddModelError("DLCodes", "Required");
            _controller.ModelState.AddModelError("Category", "Required");

            // Act
            var result = await _controller.AddDriver(newDriver);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        //CreateDriver(500)
        [Fact]
        public async void AddDriver_Exception_ReturnsServerError()
        {
            //Arrange
            var newDriver = new DriverCreationDto
            {
                OperatorName = "Operator 1",
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1",
                Category = "Primary"
            };
            _fakeDriverService.Setup(x => x.CreateDriver(newDriver))
                              .ThrowsAsync(new Exception());

            //Act
            var result = await _controller.AddDriver(newDriver);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        //GetDriversByOperatorId(200)
        [Fact]
        public async void GetDriversByOperatorId_HasOperator_ReturnsOk()
        {
            //Arrange
            var operatorId = 1;
            _fakeDriverService.Setup(service => service.GetAllByOperatorId(operatorId))
                              .ReturnsAsync(new List<DriverDto> { new DriverDto() });

            //Act
            var result = await _controller.GetDriversByOperatorId(operatorId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        //GetDriversByOperatorId(404)
        [Fact]
        public async void GetDriversByOperatorId_NoOperator_ReturnsNotFound()
        {
            //Arrange
            _fakeDriverService.Setup(x => x.GetAllByOperatorId(It.IsAny<int>()))
                                   .ReturnsAsync((List<DriverDto>)null!);

            //Act
            var result = await _controller.GetDriverById(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        //GetDriversByOperatorId(500)
        [Fact]
        public async void GetDriversByOperatorId_Exception_ReturnsServerError()
        {
            //Arrange
            _fakeDriverService.Setup(service => service.GetAllByOperatorId(It.IsAny<int>()))
                              .ThrowsAsync(new Exception());

            //Act
            var result = await _controller.GetDriversByOperatorId(It.IsAny<int>());

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        //GetDriverById(200)
        [Fact]
        public async void GetDriverById_HasDriver_ReturnsOk()
        {
            //Arrange
            var driverId = 1;
            _fakeDriverService.Setup(service => service.GetById(driverId))
                              .ReturnsAsync(new DriverDto());

            //Act
            var result = await _controller.GetDriverById(driverId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        //GetDriverById(404)
        [Fact]
        public async void GetDriverById_NoDriver_ReturnsNotFound()
        {
            //Arrange
            _fakeDriverService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync((DriverDto)null!);

            //Act
            var result = await _controller.GetDriverById(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        //GetDriverById(500)
        [Fact]
        public async void GetDriverById_Exception_ReturnsServerError()
        {
            //Arrange
            var driverId = 1;
            _fakeDriverService.Setup(service => service.GetById(driverId))
                              .ThrowsAsync(new Exception());

            //Act
            var result = await _controller.GetDriverById(driverId);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        //EditDriver(200)
        [Fact]
        public async void EditDriver_HasDriver_ReturnsOk()
        {
            //Arrange
            var driver = new DriverUpdateDto
            {
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1",
                Category = "Primary"
            };

            //Act
            var id = It.IsAny<int>();

            _fakeDriverService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new DriverDto());

            _fakeDriverService.Setup(x => x.UpdateDriver(id, driver))
                                   .ReturnsAsync(new DriverDto());

            var result = await _controller.EditDriver(id, driver);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        //EditDriver(404)
        [Fact]
        public async void EditDriver_NoDriver_ReturnsNotFound()
        {
            //Arrange
            var driver = new DriverUpdateDto
            {
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1",
                Category = "Primary"
            };

            //Act
            var id = It.IsAny<int>();

            _fakeDriverService.Setup(x => x.GetById(id))
                                   .ReturnsAsync((DriverDto)null!);

            _fakeDriverService.Setup(x => x.UpdateDriver(id, driver))
                                   .ReturnsAsync((DriverDto)null!);

            var result = await _controller.EditDriver(id, driver);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        //EditDriver(500)
        [Fact]
        public async void EditDriver_Exception_ReturnsServerError()
        {
            //Arrange
            var driver = new DriverUpdateDto
            {
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1",
                Category = "Primary"
            };

            //Act
            var id = It.IsAny<int>();

            _fakeDriverService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new DriverDto());

            _fakeDriverService.Setup(x => x.UpdateDriver(id, driver))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.EditDriver(id, driver);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        //AssignDriver(200)
        [Fact]
        public async void AssignDriver_HasDriver_ReturnsOk()
        {
            // Arrange
            var driverId = 1;
            var plateNumber = "ABC123";

            var vehicle = new VehicleDto { Id = 1, PlateNumber = plateNumber, RentStatus = false };
            var updatedDriver = new DriverDto { Id = driverId, VehicleAssigned = plateNumber };

            _fakeVehicleService.Setup(service => service.GetByPlateNumber(plateNumber))
                               .ReturnsAsync(vehicle);

            _fakeDriverService.Setup(service => service.UpdateVehicleAssigned(driverId, plateNumber))
                              .ReturnsAsync(updatedDriver);

            _fakeDriverService.Setup(service => service.UpdateVehicleAssigned(driverId, plateNumber))
                      .ReturnsAsync(updatedDriver);

            // Act
            var result = await _controller.AssignDriver(driverId, plateNumber);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(updatedDriver, okResult.Value);
        }

        //AssignDriver(404)
        [Fact]
        public async void AssignDriver_NoVehicle_ReturnsNotFound()
        {
            // Arrange
            var driverId = 1;
            var plateNumber = "ABC123";

            var vehicle = (VehicleDto)null!;

            _fakeVehicleService.Setup(service => service.GetByPlateNumber(plateNumber))
                               .ReturnsAsync(vehicle);

            // Act
            var result = await _controller.AssignDriver(driverId, plateNumber);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
        }

        //AssignDriver(409)
        [Fact]
        public async void AssignDriver_VehicleAlreadyRented_ReturnsConflict()
        {
            // Arrange
            var driverId = 1;
            var plateNumber = "ABC123";

            var vehicle = new VehicleDto { Id = 1, PlateNumber = plateNumber, RentStatus = true };

            _fakeVehicleService.Setup(service => service.GetByPlateNumber(plateNumber))
                               .ReturnsAsync(vehicle);

            // Act
            var result = await _controller.AssignDriver(driverId, plateNumber);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(StatusCodes.Status409Conflict, objectResult.StatusCode);
        }

        //AssignDriver(500)
        [Fact]
        public async void AssignDriver_Exception_ReturnsServerError()
        {
            // Arrange
            var driverId = 1;
            var plateNumber = "ABC123";

            var vehicle = new VehicleDto { Id = 1, PlateNumber = plateNumber, RentStatus = false };

            _fakeVehicleService.Setup(service => service.GetByPlateNumber(plateNumber))
                               .ReturnsAsync(vehicle);

            _fakeDriverService.Setup(service => service.UpdateVehicleAssigned(driverId, plateNumber))
                              .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.AssignDriver(driverId, plateNumber);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        //RemoveDriver(200)
        [Fact]
        public async void RemoveDriver_HasDriver_ReturnsOk()
        {
            //Arrange
            var id = It.IsAny<int>();
            _fakeDriverService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new DriverDto());

            _fakeDriverService.Setup(x => x.DeleteDriver(id))
                                   .ReturnsAsync(true);

            //Act
            var result = await _controller.RemoveDriver(id);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        //RemoveDriver(404)
        [Fact]
        public async void RemoveDriver_NoDriver_ReturnsNotFound()
        {
            //Arrange
            var id = It.IsAny<int>();
            _fakeDriverService.Setup(x => x.GetById(id))
                                   .ReturnsAsync((DriverDto)null!);

            _fakeDriverService.Setup(x => x.DeleteDriver(id))
                                   .ReturnsAsync(false);

            //Act
            var result = await _controller.RemoveDriver(id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        //RemoverDriver(500)
        [Fact]
        public async void RemoveDriver_Exception_ReturnsServerError()
        {
            //Arrange
            var id = It.IsAny<int>();
            _fakeDriverService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new DriverDto());

            _fakeDriverService.Setup(x => x.DeleteDriver(id))
                                   .ThrowsAsync(new Exception());

            //Act
            var result = await _controller.RemoveDriver(id);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }
    }
}