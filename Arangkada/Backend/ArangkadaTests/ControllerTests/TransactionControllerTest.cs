using ArangkadaAPI.Controllers;
using ArangkadaAPI.Dtos.Driver;
using ArangkadaAPI.Dtos.Transaction;
using ArangkadaAPI.Dtos.Vehicle;
using ArangkadaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;

namespace ArangkadaTests.ControllerTests
{
    public class TransactionControllerTest
    {
        private readonly TransactionsController _controller;
        private readonly Mock<ITransactionService> _fakeTransactionService;
        private readonly Mock<IVehicleService> _fakeVehicleService;
        private readonly Mock<IDriverService> _fakeDriverService;
        private readonly Mock<ILogger<TransactionsController>> _fakeLoggerService;

        public TransactionControllerTest()
        {
            _fakeTransactionService = new Mock<ITransactionService>();
            _fakeVehicleService = new Mock<IVehicleService>();
            _fakeDriverService = new Mock<IDriverService>();
            _fakeLoggerService = new Mock<ILogger<TransactionsController>>();
            _controller = new TransactionsController(_fakeTransactionService.Object, _fakeLoggerService.Object,
            _fakeDriverService!.Object, _fakeVehicleService!.Object);
        }

        [Fact]
        public async void RecordTransaction_NoExistingTransaction_ReturnsOk()
        {
            var transaction = new TransactionCreationDto
            {
                OperatorName = "Operator",
                DriverName = "Driver",
                Amount = 100,
                Date = "2021-01-01"
            };

            _fakeDriverService.Setup(x => x.GetByFullName(It.IsAny<string>()))
                              .ReturnsAsync(new DriverDto());
            _fakeDriverService.Setup(x => x.UpdateVehicleAssigned(It.IsAny<int>(), It.IsAny<string>()))
                             .ReturnsAsync(new DriverDto());
            _fakeVehicleService.Setup(x => x.GetByPlateNumber(It.IsAny<string>()))
                               .ReturnsAsync(new VehicleDto());
            _fakeVehicleService.Setup(x => x.UpdateRentStatus(It.IsAny<int>(), It.IsAny<bool>()))
                              .ReturnsAsync(new VehicleDto());

            _fakeTransactionService.Setup(x => x.CreateTransaction(transaction))
                                   .ReturnsAsync(new TransactionDto());

            var result = await _controller.RecordTransaction(transaction);

            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async void RecordTransaction_InvalidTransactionBody_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("OperatorName", "OperatorName is required");
            _controller.ModelState.AddModelError("DriverName", "DriverName is required");
            _controller.ModelState.AddModelError("Amount", "Amount is required");
            _controller.ModelState.AddModelError("Date", "Date is required");

            var transaction = new TransactionCreationDto
            {
                OperatorName = "",
                DriverName = "Driver",
                Amount = 100,
                Date = "2021-01-01"
            };

            var result = await _controller.RecordTransaction(transaction);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        [Fact]
        public async void RecordTransaction_Exception_ReturnsServerError()
        {
            var transaction = new TransactionCreationDto
            {
                OperatorName = "Operator",
                DriverName = "Driver",
                Amount = 100,
                Date = "2021-01-01"
            };
            _fakeDriverService.Setup(x => x.GetByFullName(It.IsAny<string>()))
                              .ReturnsAsync(new DriverDto());
            _fakeDriverService.Setup(x => x.UpdateVehicleAssigned(It.IsAny<int>(), It.IsAny<string>()))
                             .ReturnsAsync(new DriverDto());
            _fakeVehicleService.Setup(x => x.GetByPlateNumber(It.IsAny<string>()))
                               .ReturnsAsync(new VehicleDto());
            _fakeVehicleService.Setup(x => x.UpdateRentStatus(It.IsAny<int>(), It.IsAny<bool>()))
                              .ReturnsAsync(new VehicleDto());

            _fakeTransactionService.Setup(x => x.CreateTransaction(transaction))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.RecordTransaction(transaction);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetTransactions_HasTransactions_ReturnsOk()
        {
            _fakeTransactionService.Setup(x => x.GetAll())
                                   .ReturnsAsync(new List<TransactionDto> { new TransactionDto() });

            var result = await _controller.GetTransactions();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetTransactions_NoTransactions_ReturnsNoContent()
        {
            _fakeTransactionService.Setup(x => x.GetAll())
                                   .ReturnsAsync(new List<TransactionDto>());

            var result = await _controller.GetTransactions();

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void GetTransactions_Exception_ReturnsServerError()
        {
            _fakeTransactionService.Setup(x => x.GetAll())
                                   .ThrowsAsync(new Exception());

            var result = await _controller.GetTransactions();

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetTransactionsByOperatorId_HasTransactions_ReturnsOk()
        {
            _fakeTransactionService.Setup(x => x.GetAllByOperatorId(It.IsAny<int>()))
                                   .ReturnsAsync(new List<TransactionDto> { new TransactionDto() });

            var result = await _controller.GetTransactionsByOperatorId(It.IsAny<int>());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetTransactionsByOperatorId_NoOperator_ReturnsNotFound()
        {
            _fakeTransactionService.Setup(x => x.GetAllByOperatorId(It.IsAny<int>()))
                           .ReturnsAsync((List<TransactionDto>)null!);

            var result = await _controller.GetTransactionsByOperatorId(It.IsAny<int>());

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetTransactionsByOperatorId_Exception_ReturnsServerError()
        {
            _fakeTransactionService.Setup(x => x.GetAllByOperatorId(It.IsAny<int>()))
                                   .ThrowsAsync(new Exception());
            var result = await _controller.GetTransactionsByOperatorId(It.IsAny<int>());
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetTransactionsByDriverId_HasTransactions_ReturnsOk()
        {
            _fakeTransactionService.Setup(x => x.GetAllByDriverId(It.IsAny<int>()))
                                   .ReturnsAsync(new List<TransactionDto> { new TransactionDto() });
            var result = await _controller.GetTransactionsByDriverId(It.IsAny<int>());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetTransactionsByDriverId_NoDriver_ReturnsNotFound()
        {
            _fakeTransactionService.Setup(x => x.GetAllByDriverId(It.IsAny<int>()))
                                   .ReturnsAsync((List<TransactionDto>)null!);

            var result = await _controller.GetTransactionsByDriverId(It.IsAny<int>());

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetTransactionsByDriverId_Exception_ReturnsServerError()
        {
            _fakeTransactionService.Setup(x => x.GetAllByDriverId(It.IsAny<int>()))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.GetTransactionsByDriverId(It.IsAny<int>());

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetTransactionById_HasTransactions_ReturnsOk()
        {
            _fakeTransactionService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync(new TransactionDto());

            var result = await _controller.GetTransactionById(It.IsAny<int>());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetTransactionById_NoTransaction_ReturnsNotFound()
        {
            _fakeTransactionService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync((TransactionDto)null!);

            var result = await _controller.GetTransactionById(It.IsAny<int>());

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetTransactionById_Exception_ReturnsServerError()
        {
            _fakeTransactionService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.GetTransactionById(It.IsAny<int>());

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void EditTransaction_HasTransaction_ReturnsOk()
        {
            var transaction = new TransactionUpdateDto
            {
                Amount = 100,
                Date = "2021-01-01"
            };

            var id = It.IsAny<int>();
            _fakeTransactionService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync(new TransactionDto());
            _fakeDriverService.Setup(x => x.GetByFullName(It.IsAny<string>()))
                              .ReturnsAsync(new DriverDto());
            _fakeVehicleService.Setup(x => x.GetByPlateNumber(It.IsAny<string>()))
                               .ReturnsAsync(new VehicleDto());

            _fakeTransactionService.Setup(x => x.UpdateTransaction(id, transaction))
                                   .ReturnsAsync(new TransactionDto());

            var result = await _controller.EditTransaction(id, transaction);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void EditTransaction_InvalidTransactionUpdateBody_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Amount", "Amount is required");
            _controller.ModelState.AddModelError("Date", "Date is required");

            var transaction = new TransactionUpdateDto
            {
                Amount = 100,
                Date = null
            };

            var id = It.IsAny<int>();

            _fakeTransactionService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new TransactionDto());

            var result = await _controller.EditTransaction(id, transaction);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        [Fact]
        public async void EditTransaction_NoTransaction_ReturnsNotFound()
        {
            var transaction = new TransactionUpdateDto
            {
                Amount = 100,
                Date = "2021-01-01"
            };

            var id = It.IsAny<int>();
            _fakeTransactionService.Setup(x => x.GetById(id))
                                   .ReturnsAsync((TransactionDto)null!);

            var result = await _controller.EditTransaction(id, transaction);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void EditTransaction_Exception_ReturnsServerError()
        {
            var transaction = new TransactionUpdateDto
            {
                Amount = 100,
                Date = "2021-01-01"
            };
            var id = It.IsAny<int>();
            _fakeTransactionService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync(new TransactionDto());
            _fakeDriverService.Setup(x => x.GetByFullName(It.IsAny<string>()))
                              .ReturnsAsync(new DriverDto());
            _fakeVehicleService.Setup(x => x.GetByPlateNumber(It.IsAny<string>()))
                               .ReturnsAsync(new VehicleDto());

            _fakeTransactionService.Setup(x => x.UpdateTransaction(id, transaction))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.EditTransaction(id, transaction);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void DeleteTransaction_HasTransaction_ReturnsOk()
        {
            var id = It.IsAny<int>();
            _fakeTransactionService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new TransactionDto());

            _fakeDriverService.Setup(x => x.GetByFullName(It.IsAny<string>()))
                .ReturnsAsync(new DriverDto());

            _fakeDriverService.Setup(x => x.UpdateVehicleAssigned(It.IsAny<int>(), null))
                .ReturnsAsync(new DriverDto());

            _fakeTransactionService.Setup(x => x.DeleteTransactionById(id))
                                   .ReturnsAsync(true);

            var result = await _controller.DeleteTransaction(id);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void DeleteTransaction_NoTransaction_ReturnsNotFound()
        {
            var id = It.IsAny<int>();
            _fakeTransactionService.Setup(x => x.GetById(id))
                                   .ReturnsAsync((TransactionDto)null!);

            var result = await _controller.DeleteTransaction(id);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async Task DeleteTransaction_Exception_ReturnsServerError()
        {
            // Arrange
            var id = It.IsAny<int>();

            _fakeTransactionService.Setup(x => x.GetById(id))
                .ReturnsAsync(new TransactionDto());

            _fakeDriverService.Setup(x => x.GetByFullName(It.IsAny<string>()))
                .ReturnsAsync(new DriverDto());

            _fakeDriverService.Setup(x => x.UpdateVehicleAssigned(It.IsAny<int>(), null))
                .ReturnsAsync(new DriverDto());

            _fakeTransactionService.Setup(x => x.DeleteTransactionById(id))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.DeleteTransaction(id);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

    }
}
