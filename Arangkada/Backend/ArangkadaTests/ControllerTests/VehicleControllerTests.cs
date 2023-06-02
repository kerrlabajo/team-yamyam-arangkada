using ArangkadaAPI.Controllers;
using ArangkadaAPI.Dtos.Vehicle;
using ArangkadaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace ArangkadaTests.ControllerTests
{
    public class VehicleControllerTests
    {
        private readonly VehiclesController _controller;
        private readonly Mock<IVehicleService> _fakeVehicleService;
        private readonly Mock<ILogger<VehiclesController>> _fakeLoggerService;

        public VehicleControllerTests()
        {
            _fakeVehicleService = new Mock<IVehicleService>();
            _fakeLoggerService = new Mock<ILogger<VehiclesController>>();
            _controller = new VehiclesController(_fakeVehicleService.Object, _fakeLoggerService.Object);
        }

        //201 Created
        [Fact]
        public async void AddVehicle_NoExistingVehicle_ReturnsCreatedVehicle()
        {
            var vehicle = new VehicleCreationDto()
            {
                OperatorName = "Operator",
                CRNumber = "1234567",
                PlateNumber = "ABC123",
                BodyType = "Sedan",
                Make = "Toyota",
                RentFee = 500
            };

            _fakeVehicleService.Setup(x => x.AddVehicle(vehicle))
                               .ReturnsAsync(new VehicleDto());

            var result = await _controller.AddVehicle(vehicle);
            
            Assert.IsType<CreatedAtRouteResult>(result);
        }


        //409 Conflict
        [Fact]
        public async void AddVehicle_HasExistingVehicle_ReturnsConflict()
        {
            var newVehicle = new VehicleCreationDto()
            {
                OperatorName = "Operator",
                CRNumber = "1234567",
                PlateNumber = "ABC123",
                BodyType = "Sedan",
                Make = "Toyota",
                RentFee = 500
            };

            var existingVehicle = new VehicleDto()
            {
                OperatorName = "Operator",
                CRNumber = "1234567",
                PlateNumber = "ABC123",
                BodyType = "Sedan",
                Make = "Toyota",
                RentFee = 500
            };

            _fakeVehicleService.Setup(x => x.GetByPlateNumber(newVehicle.PlateNumber))
                               .ReturnsAsync(existingVehicle);

            var result = await _controller.AddVehicle(newVehicle);

            Assert.IsType<ConflictObjectResult>(result);
        }

        //400 Bad Request
        [Fact]
        public async void AddVehicle_InvalidVehicleBody_ReturnsBadRequest()
        {
            var vehicle = new VehicleCreationDto()
            {
                OperatorName = "Operator ",
                CRNumber = "1234567",
                PlateNumber = "ABC123",
                BodyType = "Sedan",
                Make = "Toyota",
                RentFee = 500
            };
            _controller.ModelState.AddModelError("OperatorName", "Required");
            _controller.ModelState.AddModelError("CRNumber", "Required");
            _controller.ModelState.AddModelError("PlateNumber", "Required");
            _controller.ModelState.AddModelError("BodyType", "Required");
            _controller.ModelState.AddModelError("Make", "Required");
            _controller.ModelState.AddModelError("RentFee", "Required");
            _controller.ModelState.AddModelError("RentStatus", "Required");

            var result = await _controller.AddVehicle(vehicle);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        //500 Server errror
        [Fact]
        public async void AddVehicle_Exception_ReturnsServerError()
        {
            var vehicle = new VehicleCreationDto()
            {
                OperatorName = "Operator ",
                CRNumber = "1234567",
                PlateNumber = "ABC123",
                BodyType = "Sedan",
                Make = "Toyota",
                RentFee = 500
            };

            _fakeVehicleService.Setup(x => x.AddVehicle(vehicle))
                               .ThrowsAsync(new Exception());

            var result = await _controller.AddVehicle(vehicle);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetVehiclesByOperatorId_HasVehicles_ReturnsOk()
        {
            _fakeVehicleService.Setup(x => x.GetAllByOperatorId(It.IsAny<int>()))
                                   .ReturnsAsync(new List<VehicleDto> { new VehicleDto() });

            var result = await _controller.GetVehiclesByOperatorId(It.IsAny<int>());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetVehiclesByOperatorId_NoOperator_ReturnsNotFound()
        {
            _fakeVehicleService.Setup(x => x.GetAllByOperatorId(It.IsAny<int>()))
                                   .ReturnsAsync((List<VehicleDto>)null!);

            var result = await _controller.GetVehiclesByOperatorId(It.IsAny<int>());

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetVehiclesByOperatorId_Exception_ReturnsServerError()
        {
            _fakeVehicleService.Setup(x => x.GetAllByOperatorId(It.IsAny<int>()))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.GetVehiclesByOperatorId(It.IsAny<int>());

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetVehicleById_HasVehicle_ReturnsOk()
        {
            _fakeVehicleService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync(new VehicleDto());

            var result = await _controller.GetVehicleById(It.IsAny<int>());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetVehicleById_NoVehicle_ReturnsNotFound()
        {
            _fakeVehicleService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync((VehicleDto)null!);

            var result = await _controller.GetVehicleById(It.IsAny<int>());

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetVehicleById_Exception_ReturnsServerError()
        {
            _fakeVehicleService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.GetVehicleById(It.IsAny<int>());

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void EditRentStatus_HasVehicle_ReturnsOk()
        {
           
            var id = It.IsAny<int>();
            var rentStatus = It.IsAny<bool>();

            _fakeVehicleService.Setup(x => x.GetById(id))
                             .ReturnsAsync(new VehicleDto());
            _fakeVehicleService.Setup(x => x.UpdateRentStatus(id, rentStatus))
                               .ReturnsAsync(new VehicleDto());

            var result = await _controller.EditRentStatus(id, rentStatus);  

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void EditRentStatus_Exception_ReturnsServerError()
        {

            var id = It.IsAny<int>();
            var rentStatus = It.IsAny<bool>();

            _fakeVehicleService.Setup(x => x.GetById(id))
                           .ReturnsAsync(new VehicleDto());
            _fakeVehicleService.Setup(x => x.UpdateRentStatus(id,rentStatus))
                             .ThrowsAsync(new Exception());
           
            var result = await _controller.EditRentStatus(id, rentStatus);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void EditRentStatus_NoVehicles_ReturnsNotFound()
        {

            var id = It.IsAny<int>();
            var rentStatus = It.IsAny<bool>();

            _fakeVehicleService.Setup(x => x.GetById(id))
                             .ReturnsAsync((VehicleDto)null!);

            var result = await _controller.EditRentStatus(id, rentStatus);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void EditRentFee_HasVehicle_ReturnsOk()
        {

            var id = It.IsAny<int>();
            var rentFee = It.IsAny<double>();

            _fakeVehicleService.Setup(x => x.GetById(id))
                             .ReturnsAsync(new VehicleDto());
            _fakeVehicleService.Setup(x => x.UpdateRentFee(id, rentFee))
                               .ReturnsAsync(new VehicleDto());

            var result = await _controller.EditRentFee(id, rentFee);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void EditRentFee_NoVehicle_ReturnsNotFound()
        {

            var id = It.IsAny<int>();
            var rentFee = It.IsAny<double>();

            _fakeVehicleService.Setup(x => x.GetById(id))
                             .ReturnsAsync((VehicleDto)null!);

            var result = await _controller.EditRentFee(id, rentFee);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void EditRentFee_Exception_ReturnsServerError()
        {

            var id = It.IsAny<int>();
            var rentFee = It.IsAny<double>();

            _fakeVehicleService.Setup(x => x.GetById(id))
                           .ReturnsAsync(new VehicleDto());
            _fakeVehicleService.Setup(x => x.UpdateRentFee(id, rentFee))
                             .ThrowsAsync(new Exception());

            var result = await _controller.EditRentFee(id, rentFee);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void DeleteVehicle_HasVehicle_ReturnsOk()
        {
            var id = It.IsAny<int>();

            _fakeVehicleService.Setup(x => x.GetById(id))
                          .ReturnsAsync(new VehicleDto());
            _fakeVehicleService.Setup(x => x.DeleteVehicle(id))
                             .ReturnsAsync(true);

            var result = await _controller.RemoveVehicle(id);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void DeleteVehicle_NoVehicle_ReturnsNotFound()
        {
            var id = It.IsAny<int>();

            _fakeVehicleService.Setup(x => x.GetById(id))
                          .ReturnsAsync((VehicleDto)null!); ;
           
            var result = await _controller.RemoveVehicle(id);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }
        [Fact]
        public async void DeleteVehicle_Exception_ReturnsServer()
        {
            var id = It.IsAny<int>();

            _fakeVehicleService.Setup(x => x.GetById(id))
                          .ReturnsAsync((VehicleDto)null!); 
            _fakeVehicleService.Setup(x => x.DeleteVehicle(id))
                            .ThrowsAsync(new Exception());

            var result = await _controller.RemoveVehicle(id);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

    } 
}
