using ArangkadaAPI.Controllers;
using ArangkadaAPI.Dtos.Operator;
using ArangkadaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArangkadaTests.ControllerTests
{
	public class OperatorControllerTest
	{
        private readonly OperatorsController _controller;
        private readonly Mock<IOperatorService> _fakeOperatorService;
        private readonly Mock<ILogger<OperatorsController>> _fakeLoggerService;

        public OperatorControllerTest()
        {
            _fakeOperatorService = new Mock<IOperatorService>();
            _fakeLoggerService = new Mock<ILogger<OperatorsController>>();
            _controller = new OperatorsController(_fakeOperatorService.Object, _fakeLoggerService.Object);
        }

        [Fact]
        public async void RegisterOperator_NoExistingOperator_ReturnsCreatedOperator()
        {
            var op = new OperatorCreationDto
            {
                FullName = "Adrian Jay Barcenilla",
                Username = "jaybar",
                Password = "ilovejaybar",
                Email = "jaybar@email.com"
            };

            _fakeOperatorService.Setup(x => x.CreateOperator(op))
                                   .ReturnsAsync(new OperatorDto());

            var result = await _controller.RegisterOperator(op);

            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async void RegisterOperator_HasExistingOperator_ReturnsConflict()
        {
            var newOp = new OperatorCreationDto
            {
                FullName = "Adrian Jay Barcenilla",
                Username = "jaybar",
                Password = "ilovejaybar",
                Email = "jaybar@email.com"
            };

            var existingOp = new OperatorDto
            {
                FullName = "Adrian Jay Barcenilla",
                Username = "jaybar",
                Email = "jaybar@email.com"
            };

            _fakeOperatorService.Setup(x => x.GetByFullName(newOp.FullName))
                              .ReturnsAsync(existingOp);

            var result = await _controller.RegisterOperator(newOp);

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async void RegisterOperator_InvalidOperatorBody_ReturnsBadRequest()
        {
            var op = new OperatorCreationDto
            {
                FullName = "",
                Username = "jaybar",
                Password = "ilovejaybar",
                Email = "jaybar@email.com"
            };

            _controller.ModelState.AddModelError("FullName", "FullName is required");
            _controller.ModelState.AddModelError("Username", "Username is required");
            _controller.ModelState.AddModelError("Password", "Password is required");
            _controller.ModelState.AddModelError("Email", "Email is required");

            var result = await _controller.RegisterOperator(op);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        [Fact]
        public async void RegisterOperator_Exception_ReturnsServerError()
        {
            var op = new OperatorCreationDto
            {
                FullName = "Adrian Jay Barcenilla",
                Username = "jaybar",
                Password = "ilovejaybar",
                Email = "jaybar@email.com"
            };

            _fakeOperatorService.Setup(x => x.CreateOperator(op))
                       .ThrowsAsync(new Exception());

            var result = await _controller.RegisterOperator(op);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetOperatorById_HasOperator_ReturnsOk()
        {
            _fakeOperatorService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync(new OperatorDto());

            var result = await _controller.GetOperatorById(It.IsAny<int>());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetOperatorById_NoOperator_ReturnsNotFound()
        {
            _fakeOperatorService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync((OperatorDto)null!);

            var result = await _controller.GetOperatorById(It.IsAny<int>());

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetOperatorById_Exception_ReturnsServerError()
        {
            _fakeOperatorService.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.GetOperatorById(It.IsAny<int>());

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetOperatorByUsername_HasOperator_ReturnsOk()
        {
            _fakeOperatorService.Setup(x => x.GetByUsername(It.IsAny<string>()))
                                   .ReturnsAsync(new OperatorDto());

            var result = await _controller.GetOperatorByUsername(It.IsAny<string>());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetOperatorByUsername_NoOperator_ReturnsNotFound()
        {
            _fakeOperatorService.Setup(x => x.GetByUsername(It.IsAny<string>()))
                       .ReturnsAsync((OperatorDto)null!);

            var result = await _controller.GetOperatorByUsername(It.IsAny<string>());

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetOperatorByUsername_Exception_ReturnsServerError()
        {
            _fakeOperatorService.Setup(x => x.GetByUsername(It.IsAny<string>()))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.GetOperatorByUsername(It.IsAny<string>());

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        
        [Fact]
        public async void GetPasswordById_HasPassword_ReturnsOk()
        {
            string expectedPassword = "pass";

            _fakeOperatorService.Setup(x => x.GetPasswordById(It.IsAny<int>()))
                       .ReturnsAsync(expectedPassword);

            var result = await _controller.GetPasswordById(It.IsAny<int>());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetPasswordById_NoPassword_ReturnsNotFound()
        {
            string? expectedPassword = null;

            _fakeOperatorService.Setup(x => x.GetPasswordById(It.IsAny<int>()))
                       .ReturnsAsync(expectedPassword);

            var result = await _controller.GetPasswordById(It.IsAny<int>());

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }


        [Fact]
        public async void GetPasswordById_Exception_ReturnsServerError()
        {
            _fakeOperatorService.Setup(x => x.GetPasswordById(It.IsAny<int>()))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.GetPasswordById(It.IsAny<int>());

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }
        
        [Fact]
        public async void GetIsVerifiedById_HasIsVerified_ReturnsOk()
        {
            bool expectedIsVerified = true;

            _fakeOperatorService.Setup(x => x.GetIsVerifiedById(It.IsAny<int>()))
                       .ReturnsAsync(expectedIsVerified);

            var result = await _controller.GetIsVerifiedById(It.IsAny<int>());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetIsVerifiedById_NoIsVerified_ReturnsNotFound()
        {
            bool? expectedIsVerified = null;

            _fakeOperatorService.Setup(x => x.GetIsVerifiedById(It.IsAny<int>()))
                       .ReturnsAsync(expectedIsVerified);

            var result = await _controller.GetIsVerifiedById(It.IsAny<int>());

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }
        

        [Fact]
        public async void GetIsVerifiedById_Exception_ReturnsServerError()
        {
            _fakeOperatorService.Setup(x => x.GetIsVerifiedById(It.IsAny<int>()))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.GetIsVerifiedById(It.IsAny<int>());

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void EditOperator_HasOperator_ReturnsOk()
        {
            var op = new OperatorUpdateDto
            {
                FullName = "AJ Bee",
                Username = "2021-01-01",
                Password = "2021-01-01",
                Email = "adrianjay@example.com"
            };

            var id = It.IsAny<int>();

            _fakeOperatorService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new OperatorDto());

            _fakeOperatorService.Setup(x => x.UpdateOperator(id, op))
                                   .ReturnsAsync(new OperatorDto());

            var result = await _controller.EditOperator(id, op);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void EditOperator_InvalidOperatorUpdateBody_ReturnsBadRequest()
        {
            var op = new OperatorUpdateDto
            {
                FullName = "Michael Johnson",
                Username = "mikejohnson",
                Password = "pass",
                Email = "myemail@example.com"
            };

            _controller.ModelState.AddModelError("FullName", "FullName is required");
            _controller.ModelState.AddModelError("Username", "Username is required");
            _controller.ModelState.AddModelError("Password", "Password is required");
            _controller.ModelState.AddModelError("Email", "Email is required");

            var id = It.IsAny<int>();

            _fakeOperatorService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new OperatorDto());

            var result = await _controller.EditOperator(id, op);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        }

        [Fact]
        public async void EditOperator_NoOperator_ReturnsNotFound()
        {
            var op = new OperatorUpdateDto
            {
                FullName = "Michael Johnson",
                Username = "mikejohnson",
                Password = "pass",
                Email = "myemail@example.com"
            };

            var id = It.IsAny<int>();
            _fakeOperatorService.Setup(x => x.GetById(id))
                                   .ReturnsAsync((OperatorDto)null!);

            var result = await _controller.EditOperator(id, op);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void EditOperator_Exception_ReturnsServerError()
        {
            var op = new OperatorUpdateDto
            {
                FullName = "Michael Johnson",
                Username = "mikejohnson",
                Password = "pass",
                Email = "myemail@example.com"
            };

            var id = It.IsAny<int>();
            _fakeOperatorService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new OperatorDto());

            _fakeOperatorService.Setup(x => x.UpdateOperator(id, op))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.EditOperator(id, op);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void UpdateVerification_HasOperator_ReturnsOk()
        {
            var id = It.IsAny<int>();
            bool verificationStatus = true;

            _fakeOperatorService.Setup(x => x.GetById(id))
                       .ReturnsAsync(new OperatorDto());

            _fakeOperatorService.Setup(x => x.UpdateIsVerified(id, verificationStatus))
                                   .ReturnsAsync(new OperatorDto());

            var result = await _controller.UpdateVerification(id, verificationStatus);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void UpdateVerification_NoOperator_ReturnsNotFound()
        {
            var id = It.IsAny<int>();
            bool verificationStatus = true;

            _fakeOperatorService.Setup(x => x.GetById(id))
                                   .ReturnsAsync((OperatorDto)null!);

            var result = await _controller.UpdateVerification(id, verificationStatus);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void UpdateVerification_Exception_ReturnsServerError()
        {
            var id = It.IsAny<int>();
            bool verificationStatus = true;

            _fakeOperatorService.Setup(x => x.GetById(id))
                       .ReturnsAsync(new OperatorDto());

            _fakeOperatorService.Setup(x => x.UpdateIsVerified(id, verificationStatus))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.UpdateVerification(id, verificationStatus);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void DeleteOperator_HasOperator_ReturnsOk()
        {
            var id = It.IsAny<int>();
            _fakeOperatorService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new OperatorDto());

            _fakeOperatorService.Setup(x => x.DeleteOperator(id))
                                   .ReturnsAsync(true);

            var result = await _controller.DeleteOperator(id);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void DeleteOperator_NoOperator_ReturnsNotFound()
        {
            var id = It.IsAny<int>();
            _fakeOperatorService.Setup(x => x.GetById(id))
                                   .ReturnsAsync((OperatorDto)null!);

            var result = await _controller.DeleteOperator(id);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public async void DeleteOperator_Exception_ReturnsServerError()
        {
            var id = It.IsAny<int>();
            _fakeOperatorService.Setup(x => x.GetById(id))
                                   .ReturnsAsync(new OperatorDto());

            _fakeOperatorService.Setup(x => x.DeleteOperator(id))
                                   .ThrowsAsync(new Exception());

            var result = await _controller.DeleteOperator(id);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }


    }
}
