using ArangkadaAPI.Dtos.Driver;
using ArangkadaAPI.Dtos.Operator;
using ArangkadaAPI.Models;
using ArangkadaAPI.Repositories;
using ArangkadaAPI.Services;
using AutoMapper;
using Moq;

namespace ArangkadaTests.ServicesTests
{
    public class IDriverServicesTest
    {
        private readonly IDriverService _driverService;
        private readonly Mock<IDriverRepository> _fakeDriverRepository;
        private readonly Mock<IMapper> _fakeMapper;

        public IDriverServicesTest()
        {
            _fakeDriverRepository = new Mock<IDriverRepository>();
            _fakeMapper = new Mock<IMapper>();
            _driverService = new DriverService(_fakeDriverRepository.Object, _fakeMapper.Object);
        }
        
        [Fact]
        public async void CreateDriver_ValidDriverToCreate_ReturnsDriverDto()
        {
            _fakeMapper.Setup(x => x.Map<Driver>(It.IsAny<DriverCreationDto>()))
                       .Returns(new Driver());

            _fakeDriverRepository.Setup(x => x.CreateDriver(It.IsAny<Driver>()))
                                   .ReturnsAsync(1);

            _fakeMapper.Setup(x => x.Map<DriverDto>(It.IsAny<Driver>()))
                       .Returns(new DriverDto());

            var result = await _driverService.CreateDriver(It.IsAny<DriverCreationDto>());

            Assert.NotNull(result);
            Assert.IsType<DriverDto>(result);
        }
        
        [Fact]
        public async void CreateDriver_Exception_ReturnsNull()
        {
            var invalidDriverToCreate = new DriverCreationDto
            {
                OperatorName = "Operator 1",
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1"
            };

            _fakeDriverRepository.Setup(x => x.CreateDriver(It.IsAny<Driver>()))
                           .Throws(new Exception("Simulated Exception"));
            
            var result = await _driverService.CreateDriver(invalidDriverToCreate);
            
            Assert.Null(result);
        }
        
        [Fact]
        public async void GetAllByOperatorId_HasOperator_ReturnsDriverDtoList()
        {
            var driverModelList = new List<Driver>
            {
                new Driver
                {
                    Id = 1,
                    OperatorName = "Operator 1",
                    VehicleAssigned = null,
                    FullName = "John William",
                    Address = "Tres",
                    ContactNumber = "09221321",
                    LicenseNumber = "161230129391",
                    ExpirationDate = "21/12/2001",
                    DLCodes = "A1"
                },
                new Driver
                {
                    Id = 2,
                    OperatorName = "Operator 2",
                    VehicleAssigned = null,
                    FullName = "John William",
                    Address = "Tres",
                    ContactNumber = "09221321",
                    LicenseNumber = "161230129391",
                    ExpirationDate = "21/12/2001",
                    DLCodes = "A1"
                }
            };

            var expectedDriverDtoList = new List<DriverDto>
            {
                new DriverDto
                {
                    Id = 1,
                    OperatorName = "Operator 1",
                    VehicleAssigned = null,
                    FullName = "John William",
                    Address = "Tres",
                    ContactNumber = "09221321",
                    LicenseNumber = "161230129391",
                    ExpirationDate = "21/12/2001",
                    DLCodes = "A1"
                },
                new DriverDto
                {
                    Id = 2,
                    OperatorName = "Operator 2",
                    VehicleAssigned = null,
                    FullName = "John William",
                    Address = "Tres",
                    ContactNumber = "09221321",
                    LicenseNumber = "161230129391",
                    ExpirationDate = "21/12/2001",
                    DLCodes = "A1"
                }
            };

            _fakeDriverRepository.Setup(x => x.GetAllByOperatorId(1))
                                 .ReturnsAsync(driverModelList);

            _fakeMapper.Setup(x => x.Map<IEnumerable<DriverDto>>(driverModelList))
                       .Returns(expectedDriverDtoList);
            
            var result = await _driverService.GetAllByOperatorId(1);
            
            Assert.NotNull(result);
            Assert.Equal(expectedDriverDtoList, result);
        }
        
        [Fact]
        public async void GetAllByOperatorId_NoOperator_ReturnsNull()
        {
            int operatorId = 1;
            List<Driver>? driverModelList = null;
            _fakeDriverRepository.Setup(x => x.GetAllByOperatorId(operatorId))
                                 .ReturnsAsync(driverModelList!);
            
            var result = await _driverService.GetAllByOperatorId(operatorId);
            
            Assert.Null(result);
        }
        
        [Fact]
        public async void GetAllByOperatorId_Exception_ReturnsNull()
        {
            _fakeDriverRepository.Setup(x => x.GetAllByOperatorId(1))
                                 .Throws(new Exception("Simulated Exception"));
            
            var result = await _driverService.GetAllByOperatorId(1);
            
            Assert.Null(result);
        }
        
        [Fact]
        public async void GetById_HasDriver_ReturnsDriverDto()
        {
            _fakeDriverRepository.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync(new Driver());

            _fakeMapper.Setup(x => x.Map<DriverDto>(It.IsAny<Driver>()))
                       .Returns(new DriverDto());

            var result = await _driverService.GetById(1);

            Assert.NotNull(result);
            Assert.IsType<DriverDto>(result);
        }
        
        [Fact]
        public async void GetById_NoDriver_ReturnsNull()
        {
            int driverId = 1;
            Driver? driverModel = null;
            _fakeDriverRepository.Setup(x => x.GetById(driverId))
                                 .ReturnsAsync(driverModel!);
     
            var result = await _driverService.GetById(driverId);
    
            Assert.Null(result);
        }
        
        [Fact]
        public async void GetById_Exception_ReturnsNull()
        {
            _fakeDriverRepository.Setup(x => x.GetById(1))
                                 .Throws(new Exception("Simulated Exception"));
       
            var result = await _driverService.GetById(1);
        
            Assert.Null(result);
        }
        
        [Fact]
        public async void GetByFullName_HasDriver_ReturnsDriverDto()
        {
            _fakeDriverRepository.Setup(x => x.GetByFullName(It.IsAny<string>()))
                                   .ReturnsAsync(new Driver());

            _fakeMapper.Setup(x => x.Map<DriverDto>(It.IsAny<Driver>()))
                       .Returns(new DriverDto());

            var result = await _driverService.GetByFullName(It.IsAny<string>());

            Assert.NotNull(result);
            Assert.IsType<DriverDto>(result);
        }
        
        [Fact]
        public async void GetByFullName_NoDriver_ReturnsNull()
        {
            _fakeDriverRepository.Setup(x => x.GetByFullName(It.IsAny<string>()))
                                   .ReturnsAsync((Driver)null!);

            var result = await _driverService.GetByFullName(It.IsAny<string>());

            Assert.Null(result);
        }
        
        [Fact]
        public async void GetByFullName_Exception_ReturnsNull()
        {
            _fakeDriverRepository.Setup(x => x.GetByFullName(It.IsAny<string>()))
                                   .Throws(new Exception("Simulated exception"));

            var result = await _driverService.GetByFullName(It.IsAny<string>());

            Assert.Null(result);
        }
        
        [Fact]
        public async void UpdateDriver_HasDriver_ReturnsDriverDto()
        {
            int driverId = 1;

            var driverToUpdate = new DriverUpdateDto
            {
                FullName = "Updated Full Name",
                Address = "Updated Address",
                ContactNumber = "Updated Contact Number",
                LicenseNumber = "Updated License Number",
                ExpirationDate = "Updated Expiration Date",
                DLCodes = "Updated DL Codes"
            };

            var existingDriver = new Driver
            {
                Id = driverId,
                OperatorName = "Operator 1",
                VehicleAssigned = null,
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1"
            };

            var updatedDriver = new Driver
            {
                Id = driverId,
                OperatorName = "Updated Operator",
                VehicleAssigned = null,
                FullName = "Updated Full Name",
                Address = "Updated Address",
                ContactNumber = "Updated Contact Number",
                LicenseNumber = "Updated License Number",
                ExpirationDate = "Updated Expiration Date",
                DLCodes = "Updated DL Codes"
            };

            var expectedDriverDto = new DriverDto
            {
                Id = driverId,
                OperatorName = "Updated Operator",
                VehicleAssigned = null,
                FullName = "Updated Full Name",
                Address = "Updated Address",
                ContactNumber = "Updated Contact Number",
                LicenseNumber = "Updated License Number",
                ExpirationDate = "Updated Expiration Date",
                DLCodes = "Updated DL Codes"
            };

            _fakeDriverRepository.Setup(x => x.GetById(driverId))
                           .ReturnsAsync(existingDriver);

            _fakeMapper.Setup(x => x.Map<Driver>(driverToUpdate))
                       .Returns(existingDriver);

            _fakeDriverRepository.Setup(x => x.UpdateDriver(driverId, existingDriver))
                           .ReturnsAsync(updatedDriver);

            _fakeMapper.Setup(x => x.Map<DriverDto>(updatedDriver))
                       .Returns(expectedDriverDto);
            
            var result = await _driverService.UpdateDriver(driverId, driverToUpdate);
            
            Assert.Equal(expectedDriverDto, result);
        }
        
        [Fact]
        public async void UpdateDriver_Exception_ReturnsNull()
        {
            int driverId = 1;
            var driverToUpdate = new DriverUpdateDto
            {
                FullName = "Updated Full Name",
                Address = "Updated Address",
                ContactNumber = "Updated Contact Number",
                LicenseNumber = "Updated License Number",
                ExpirationDate = "Updated Expiration Date",
                DLCodes = "Updated DL Codes"
            };
            var existingDriver = new Driver
            {
                Id = driverId,
                OperatorName = "Operator 1",
                VehicleAssigned = null,
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1"
            };
            _fakeMapper.Setup(x => x.Map<Driver>(driverToUpdate))
                       .Returns(existingDriver);
            _fakeDriverRepository.Setup(x => x.UpdateDriver(driverId, existingDriver))
                                 .Throws(new Exception("Simulated Exception"));
            
            var result = await _driverService.UpdateDriver(driverId, driverToUpdate);
        
            Assert.Null(result);
        }
        
        [Fact]
        public async void UpdateVehicleAssigned_HasDriver_ReturnsDriverDto()
        {
            int driverId = 1;
            string plateNumberAssigned = "ABC123";

            var existingDriver = new Driver
            {
                Id = driverId,
                OperatorName = "Operator 1",
                VehicleAssigned = null,
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1"
            };

            var updatedDriver = new Driver
            {
                Id = driverId,
                OperatorName = "Operator 1",
                VehicleAssigned = "ABC123",
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1"
            };

            var expectedDriverDto = new DriverDto
            {
                Id = driverId,
                OperatorName = "Operator 1",
                VehicleAssigned = "ABC123",
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1"
            };

            _fakeDriverRepository.Setup(x => x.GetById(driverId))
                           .ReturnsAsync(existingDriver);

            _fakeDriverRepository.Setup(x => x.UpdateVehicleAssigned(driverId, plateNumberAssigned))
                           .ReturnsAsync(updatedDriver);

            _fakeMapper.Setup(x => x.Map<DriverDto>(updatedDriver))
                       .Returns(expectedDriverDto);
        
            var result = await _driverService.UpdateVehicleAssigned(driverId, plateNumberAssigned);

            Assert.Equal(expectedDriverDto, result);
        }
        
        [Fact]
        public async void UpdateVehicleAssigned_Exception_ReturnsNull()
        {
            int driverId = 1;
            string plateNumberAssigned = "ABC123";
            var existingDriver = new Driver
            {
                Id = driverId,
                OperatorName = "Operator 1",
                VehicleAssigned = null,
                FullName = "John William",
                Address = "Tres",
                ContactNumber = "09221321",
                LicenseNumber = "161230129391",
                ExpirationDate = "21/12/2001",
                DLCodes = "A1"
            };
            _fakeDriverRepository.Setup(x => x.UpdateVehicleAssigned(driverId, plateNumberAssigned))
                                 .Throws(new Exception("Simulated Exception"));
 
            var result = await _driverService.UpdateVehicleAssigned(driverId, plateNumberAssigned);
 
            Assert.Null(result);
        }
        
        [Fact]
        public async void DeleteDriver_HasDriver_ReturnsTrue()
        {
            int driverId = 1;

            _fakeDriverRepository.Setup(x => x.DeleteDriver(driverId))
                           .ReturnsAsync(true);
            
            var result = await _driverService.DeleteDriver(driverId);
            
            Assert.True(result);
        }
        
        [Fact]
        public async void DeleteDriver_Exception_ReturnsFalse()
        {
            int driverId = 1;
            _fakeDriverRepository.Setup(x => x.DeleteDriver(driverId))
                                 .Throws(new Exception("Simulated Exception"));
     
            var result = await _driverService.DeleteDriver(driverId);

            Assert.False(result);
        }

    }
}
