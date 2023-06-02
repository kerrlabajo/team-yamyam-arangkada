using ArangkadaAPI.Dtos.Vehicle;
using ArangkadaAPI.Mappings;
using ArangkadaAPI.Models;
using AutoMapper;

namespace ArangkadaTests.MappingsTests
{
    public class VehicleMappingTests
    {
        private readonly IMapper _mapper;

        public VehicleMappingTests() {

            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new VehicleMappings()));

            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public void VehicleMappings_ValidConfiguration()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<VehicleMappings>();
            });

            // Act & Assert
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void CreateMap_VehicleToVehicleDto_ValidMapping()
        {
            var vehicle = new Vehicle
            {
                Id = 1,
                OperatorName = "Operator",
                CRNumber = "CRNumber",
                PlateNumber = "PlateNumber",
                BodyType = "BodyType",
                Make = "Make",
                RentFee = 500,
                RentStatus = true
            };

            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);

            Assert.Equal(vehicle.Id, vehicleDto.Id);
            Assert.Equal(vehicle.OperatorName, vehicleDto.OperatorName);
            Assert.Equal(vehicle.CRNumber, vehicleDto.CRNumber);
            Assert.Equal(vehicle.PlateNumber, vehicleDto.PlateNumber);
            Assert.Equal(vehicle.BodyType, vehicleDto.BodyType);
            Assert.Equal(vehicle.Make, vehicleDto.Make);
            Assert.Equal(vehicle.RentFee, vehicleDto.RentFee);
            Assert.Equal(vehicle.RentStatus, vehicleDto.RentStatus);           
        }

        [Fact]
        public void CreateMap_VehicleCreationDtoToVehicle_ValidMapping()
        {
            var vehicleCreationDto = new VehicleCreationDto
            {
                OperatorName = "Operator",
                CRNumber = "CRNumber",
                PlateNumber = "PlateNumber",
                BodyType = "BodyType",
                Make = "Make",
                RentFee = 500
            };

            var vehicle = _mapper.Map<Vehicle>(vehicleCreationDto);

            Assert.Equal(vehicleCreationDto.OperatorName, vehicle.OperatorName);
            Assert.Equal(vehicleCreationDto.CRNumber, vehicle.CRNumber);
            Assert.Equal(vehicleCreationDto.PlateNumber, vehicle.PlateNumber);
            Assert.Equal(vehicleCreationDto.BodyType, vehicle.BodyType);
            Assert.Equal(vehicleCreationDto.Make, vehicle.Make);
            Assert.Equal(vehicleCreationDto.RentFee, vehicle.RentFee);
        }

    }
}

