using ArangkadaAPI.Dtos.Driver;
using ArangkadaAPI.Mappings;
using ArangkadaAPI.Models;
using AutoMapper;

namespace ArangkadaTests.MappingsTests
{
    public class DriverMappingsTest
    {
        private readonly IMapper _mapper;

        public DriverMappingsTest()
        {
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new DriverMappings()));

            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public void DriverMappings_ValidConfiguration()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DriverMappings>();
            });

            // Act & Assert
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void CreateMap_DriverToDriverDto_ValidMapping()
        {
            // Arrange
            var driver = new Driver
            {
                Id = 1,
                OperatorName = "OperatorName",
                VehicleAssigned = "VehicleAssigned",
                FullName = "FullName",
                Address = "Address",
                ContactNumber = "ContactNumber",
                LicenseNumber = "LicenseNumber",
                ExpirationDate = "ExpirationDate",
                DLCodes = "DLCodes",
                Category = "Category"
            };

            // Act
            var driverDto = _mapper.Map<DriverDto>(driver);

            // Assert
            Assert.Equal(driver.Id, driverDto.Id);
            Assert.Equal(driver.OperatorName, driverDto.OperatorName);
            Assert.Equal(driver.VehicleAssigned, driverDto.VehicleAssigned);
            Assert.Equal(driver.FullName, driverDto.FullName);
            Assert.Equal(driver.Address, driverDto.Address);
            Assert.Equal(driver.ContactNumber, driverDto.ContactNumber);
            Assert.Equal(driver.LicenseNumber, driverDto.LicenseNumber);
            Assert.Equal(driver.ExpirationDate, driverDto.ExpirationDate);
            Assert.Equal(driver.DLCodes, driverDto.DLCodes);
            Assert.Equal(driver.Category, driverDto.Category);
        }

        [Fact]
        public void CreateMap_DriverCreationDtoToDriver_ValidMapping()
        {
            // Arrange
            var driverCreationDto = new DriverCreationDto
            {
                OperatorName = "Operator1",
                FullName = "John Doe",
                Address = "123 Main St",
                ContactNumber = "555-1234",
                LicenseNumber = "ABC123",
                ExpirationDate = "2023-12-31",
                DLCodes = "DL1,DL2",
                Category = "Primary"
            };

            // Act
            var driver = _mapper.Map<Driver>(driverCreationDto);

            // Assert
            Assert.Equal(driverCreationDto.OperatorName, driver.OperatorName);
            Assert.Equal(driverCreationDto.FullName, driver.FullName);
            Assert.Equal(driverCreationDto.Address, driver.Address);
            Assert.Equal(driverCreationDto.ContactNumber, driver.ContactNumber);
            Assert.Equal(driverCreationDto.LicenseNumber, driver.LicenseNumber);
            Assert.Equal(driverCreationDto.ExpirationDate, driver.ExpirationDate);
            Assert.Equal(driverCreationDto.DLCodes, driver.DLCodes);
            Assert.Equal(driverCreationDto.Category, driver.Category);
        }
        
        [Fact]
        public void CreateMap_DriverUpdateDtoToDriver_ValidMapping()
        {
            // Arrange
            var driverUpdateDto = new DriverUpdateDto
            {
                FullName = "John Doe",
                Address = "123 Main St",
                ContactNumber = "555-1234",
                LicenseNumber = "ABC123",
                ExpirationDate = "2023-12-31",
                DLCodes = "DL1,DL2",
                Category = "Extra"
            };

            // Act
            var driver = _mapper.Map<Driver>(driverUpdateDto);

            // Assert
            Assert.Equal(driverUpdateDto.FullName, driver.FullName);
            Assert.Equal(driverUpdateDto.Address, driver.Address);
            Assert.Equal(driverUpdateDto.ContactNumber, driver.ContactNumber);
            Assert.Equal(driverUpdateDto.LicenseNumber, driver.LicenseNumber);
            Assert.Equal(driverUpdateDto.ExpirationDate, driver.ExpirationDate);
            Assert.Equal(driverUpdateDto.DLCodes, driver.DLCodes);
            Assert.Equal(driverUpdateDto.Category, driver.Category);
        }
    }
}
