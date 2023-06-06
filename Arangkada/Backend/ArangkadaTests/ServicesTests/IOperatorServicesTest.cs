using ArangkadaAPI.Dtos.Operator;
using ArangkadaAPI.Models;
using ArangkadaAPI.Repositories;
using ArangkadaAPI.Services;
using AutoMapper;
using Moq;

namespace ArangkadaTests.ServicesTests
{
	public class IOperatorServicesTest : Profile
	{
        private readonly IOperatorService _operatorService;
        private readonly Mock<IEmailService> _fakeEmailService;
        private readonly Mock<IOperatorRepository> _fakeOperatorRepository;
        private readonly Mock<IMapper> _fakeMapper;

        public IOperatorServicesTest()
        {
            _fakeOperatorRepository = new Mock<IOperatorRepository>();
            _fakeMapper = new Mock<IMapper>();
            _fakeEmailService = new Mock<IEmailService>();
            _operatorService = new OperatorService(_fakeOperatorRepository.Object, _fakeMapper.Object, _fakeEmailService.Object);
        }

        [Fact]
        public async Task CreateOperator_ValidOperatorToCreate_ReturnsOperatorDto()
        {
            var operatorToCreate = new OperatorCreationDto();
            var operatorModel = new Operator();
            var operatorDto = new OperatorDto();

            _fakeMapper.Setup(x => x.Map<Operator>(operatorToCreate)).Returns(operatorModel);

            _fakeOperatorRepository.Setup(x => x.CreateOperator(operatorModel)).ReturnsAsync(1);

            _fakeOperatorRepository.Setup(x => x.GetById(1)).ReturnsAsync(operatorModel);

            _fakeMapper.Setup(x => x.Map<OperatorDto>(operatorModel)).Returns(operatorDto);

            _fakeEmailService.Setup(x => x.SendEmailVerification(operatorDto.Email, operatorModel.VerificationCode))
                             .Returns(Task.CompletedTask);

            var result = await _operatorService.CreateOperator(operatorToCreate);

            Assert.NotNull(result);
            Assert.IsType<OperatorDto>(result);
        }


        [Fact]
        public async void CreateOperator_Exception_ReturnsNull()
        {
            var operatorToCreate = new OperatorCreationDto
            {
                FullName = "Test Operator",
                Username = "testoperator",
                Password = "testpassword",
                Email = "testoperator@example.com"
            };

            _fakeMapper.Setup(x => x.Map<Operator>(operatorToCreate))
                .Throws(new Exception("Simulated exception"));

            var result = await _operatorService.CreateOperator(operatorToCreate);

            Assert.Null(result);
        }


        [Fact]
        public async void GetById_HasOperator_ReturnsOperatorDto()
        {
            _fakeOperatorRepository.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync(new Operator());

            _fakeMapper.Setup(x => x.Map<OperatorDto>(It.IsAny<Operator>()))
                       .Returns(new OperatorDto());

            var result = await _operatorService.GetById(1);

            Assert.NotNull(result);
            Assert.IsType<OperatorDto>(result);
        }

        [Fact]
        public async void GetById_NoOperator_ReturnsNull()
        {
            _fakeOperatorRepository.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync((Operator)null!);

            var result = await _operatorService.GetById(It.IsAny<int>());

            Assert.Null(result);
        }


        [Fact]
        public async void GetById_Exception_ReturnsNull()
        {
            _fakeOperatorRepository.Setup(x => x.GetById(It.IsAny<int>()))
                                   .Throws(new Exception("Simulated exception"));

            var result = await _operatorService.GetById(It.IsAny<int>());

            Assert.Null(result);
        }


        [Fact]
        public async void GetByFullName_HasOperator_ReturnsOperatorDto()
        {
            _fakeOperatorRepository.Setup(x => x.GetByFullName(It.IsAny<string>()))
                                   .ReturnsAsync(new Operator());

            _fakeMapper.Setup(x => x.Map<OperatorDto>(It.IsAny<Operator>()))
                       .Returns(new OperatorDto());

            var result = await _operatorService.GetByFullName(It.IsAny<string>());

            Assert.NotNull(result);
            Assert.IsType<OperatorDto>(result);
        }


        [Fact]
        public async void GetByFullName_NoOperator_ReturnsNull()
        {
            _fakeOperatorRepository.Setup(x => x.GetByFullName(It.IsAny<string>()))
                                   .ReturnsAsync((Operator)null!);

            var result = await _operatorService.GetByFullName(It.IsAny<string>());

            Assert.Null(result);
        }


        [Fact]
        public async void GetByFullName_Exception_ReturnsNull()
        {
            _fakeOperatorRepository.Setup(x => x.GetByFullName(It.IsAny<string>()))
                                   .Throws(new Exception("Simulated exception"));

            var result = await _operatorService.GetByFullName(It.IsAny<string>());

            Assert.Null(result);
        }


        [Fact]
        public async void GetByUsername_HasOperator_ReturnsOperatorDto()
        {
            _fakeOperatorRepository.Setup(x => x.GetByUsername(It.IsAny<string>()))
                                   .ReturnsAsync(new Operator());

            _fakeMapper.Setup(x => x.Map<OperatorDto>(It.IsAny<Operator>()))
                       .Returns(new OperatorDto());

            var result = await _operatorService.GetByUsername(It.IsAny<string>());

            Assert.NotNull(result);
            Assert.IsType<OperatorDto>(result);
        }


        [Fact]
        public async void GetByUsername_NoOperator_ReturnsNull()
        {
            _fakeOperatorRepository.Setup(x => x.GetByUsername(It.IsAny<string>()))
                                   .ReturnsAsync((Operator)null!);

            var result = await _operatorService.GetByUsername(It.IsAny<string>());

            Assert.Null(result);
        }

        [Fact]
        public async void GetByUsername_Exception_ReturnsNull()
        {

            _fakeOperatorRepository.Setup(x => x.GetByUsername(It.IsAny<string>()))
                                   .Throws(new Exception("Simulated exception"));

            var result = await _operatorService.GetByUsername(It.IsAny<string>());

            Assert.Null(result);
        }


        [Fact]
        public async void GetPasswordById_HasOperator_ReturnsEncryptedPassword()
        {
            int id = 1;
            string passwordString = "password123";

            _fakeOperatorRepository.Setup(x => x.GetPasswordById(id))
                                   .ReturnsAsync(passwordString);

            var result = await _operatorService.GetPasswordById(id);

            Assert.True(BCrypt.Net.BCrypt.Verify(passwordString, result));
        }


        [Fact]
        public async void GetPasswordById_NoOperator_ReturnsNull()
        {
            int id = 1;

            _fakeOperatorRepository.Setup(x => x.GetPasswordById(id))
                           .ReturnsAsync((string)null!);

            var result = await _operatorService.GetPasswordById(id);

            Assert.Null(result);
        }

        [Fact]
        public async void GetPasswordById_Exception_ReturnsNull()
        {
            int id = 1;

            _fakeOperatorRepository.Setup(x => x.GetPasswordById(id))
                           .Throws(new Exception("Simulated exception"));

            var result = await _operatorService.GetPasswordById(id);

            Assert.Null(result);
        }


        [Fact]
        public async void GetIsVerifiedById_HasOperator_ReturnsTrue()
        {
            int id = 1;

            _fakeOperatorRepository.Setup(x => x.GetVerificationStatusById(id))
                                   .ReturnsAsync(true);

            var result = await _operatorService.GetVerificationStatusById(id);

            Assert.True(result);
        }


        [Fact]
        public async void GetIsVerifiedById_Exception_ReturnsNull()
        {
            int id = 1;

            _fakeOperatorRepository.Setup(x => x.GetVerificationStatusById(id))
                                   .ThrowsAsync(new Exception("Test Exception"));

            var result = await _operatorService.GetVerificationStatusById(id);

            Assert.Null(result);
        }


        [Fact]
        public async void UpdateOperator_HasOperator_ReturnsOperatorDto()
        {
            int operatorId = 1;

            var operatorToUpdate = new OperatorUpdateDto
            {
                FullName = "Updated Full Name",
                Username = "updatedUsername",
                Password = "upd4t3dp4ssw0rd",
                Email = "updated@mail.com",
            };

            var expectedOperatorDto = new OperatorDto
            {
                FullName = "Updated Full Name",
                Username = "updatedUsername",
                Email = "updated@mail.com",
                VerificationStatus = false,
                Vehicles = 0,
                Drivers = 0
            };

            _fakeMapper.Setup(x => x.Map<Operator>(operatorToUpdate))
                       .Returns(new Operator { Id = operatorId });

            _fakeOperatorRepository.Setup(x => x.UpdateOperator(operatorId, It.IsAny<Operator>()))
                                   .ReturnsAsync(new Operator { Id = operatorId });

            _fakeMapper.Setup(x => x.Map<OperatorDto>(It.IsAny<Operator>()))
                       .Returns(expectedOperatorDto);

            var result = await _operatorService.UpdateOperator(operatorId, operatorToUpdate);

            Assert.Equal(expectedOperatorDto, result);
        }

        [Fact]
        public async void UpdateOperator_Exception_ReturnsNull()
        {
            int operatorId = 1;

            var operatorToUpdate = new OperatorUpdateDto
            {
                FullName = "Updated Full Name",
                Username = "updatedUsername",
                Password = "upd4t3dp4ssw0rd",
                Email = "updated@mail.com",
            };

            _fakeOperatorRepository.Setup(x => x.UpdateOperator(operatorId, It.IsAny<Operator>()))
                                   .ThrowsAsync(new System.Exception("Test Exception"));

            var result = await _operatorService.UpdateOperator(operatorId, operatorToUpdate);

            Assert.Null(result);
        }


        [Fact]
        public async void UpdateIsVerified_HasOperator_ReturnsOperatorDto()
        {
            int operatorId = 1;
            bool isVerified = true;

            var updatedOperator = new Operator
            {
                Id = operatorId,
                FullName = "Taka Moriuchi",
                Username = "takamoriuchi",
                Password = "0ne0kr0ck",
                Email = "taka.oor@email.com",
                VerificationStatus = isVerified,
                Vehicles = 0,
                Drivers = 0
            };

            var expectedOperatorDto = new OperatorDto
            {
                Id = operatorId,
                FullName = "Taka Moriuchi",
                Username = "takamoriuchi",
                Email = "taka.oor@email.com",
                VerificationStatus = isVerified,
                Vehicles = 0,
                Drivers = 0
            };

            _fakeOperatorRepository.Setup(x => x.UpdateIsVerified(operatorId, isVerified))
                                   .ReturnsAsync(updatedOperator);

            _fakeOperatorRepository.Setup(x => x.GetById(operatorId))
                                   .ReturnsAsync(updatedOperator);

            _fakeMapper.Setup(x => x.Map<OperatorDto>(updatedOperator))
                       .Returns(expectedOperatorDto);

            var result = await _operatorService.UpdateIsVerified(operatorId, isVerified);

            Assert.Equal(expectedOperatorDto, result);
        }


        [Fact]
        public async void UpdateIsVerified_Exception_ReturnsNull()
        {
            int operatorId = 1;
            bool isVerified = true;
            _fakeOperatorRepository.Setup(x => x.UpdateIsVerified(operatorId, isVerified))
                                   .ThrowsAsync(new System.Exception("Test Exception"));

            var result = await _operatorService.UpdateIsVerified(operatorId, isVerified);

            Assert.Null(result);
        }


        [Fact]
        public async void DeleteOperator_HasOperator_ReturnsTrue()
        {
            int operatorId = 1;

            _fakeOperatorRepository.Setup(x => x.DeleteOperator(operatorId))
                           .ReturnsAsync(true);

            var result = await _operatorService.DeleteOperator(operatorId);

            Assert.True(result);
        }


        [Fact]
        public async void DeleteOperator_Exception_ReturnsFalse()
        {
            int operatorId = 1;
            _fakeOperatorRepository.Setup(x => x.DeleteOperator(operatorId))
                           .ThrowsAsync(new Exception("Simulated exception"));

            var result = await _operatorService.DeleteOperator(operatorId);

            Assert.False(result);
        }
    }
}
