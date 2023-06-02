using ArangkadaAPI.Dtos.Vehicle;
using ArangkadaAPI.Models;
using ArangkadaAPI.Repositories;
using ArangkadaAPI.Services;
using AutoMapper;
using Moq;


namespace ArangkadaTests.ServicesTests
{
    public class IVehicleServicesTests
    {
        private readonly IVehicleService _vehicleService;
        private readonly Mock<IVehicleRepository> _fakeVehicleRepository;
        private readonly Mock<IMapper> _fakeMapper;

        public IVehicleServicesTests()
        {
            _fakeVehicleRepository = new Mock<IVehicleRepository>();
            _fakeMapper = new Mock<IMapper>();
            _vehicleService = new VehicleService(_fakeVehicleRepository.Object, _fakeMapper.Object);
        }

        [Fact]
        public async void AddVehicle_ValidVehicleToCreate_ReturnsVehicleDto()
        {
            _fakeMapper.Setup(x => x.Map<Vehicle>(It.IsAny<VehicleCreationDto>()))
                      .Returns(new Vehicle());

            _fakeVehicleRepository.Setup(x => x.AddVehicle(It.IsAny<Vehicle>()))
                                   .ReturnsAsync(1);

            _fakeMapper.Setup(x => x.Map<VehicleDto>(It.IsAny<Vehicle>()))
                       .Returns(new VehicleDto());

            var result = await _vehicleService.AddVehicle(It.IsAny<VehicleCreationDto>());

            Assert.NotNull(result);
            Assert.IsType<VehicleDto>(result);
        }

        [Fact]
        public async void AddVehicle_Exception_ReturnsNull()
        {
            var invalidVehicleToCreate = new VehicleCreationDto
            {
                OperatorName = "Operator",
                CRNumber = "CRNumber",
                PlateNumber = "PlateNumber",
                BodyType = "BodyType",
                Make = "Make",
                RentFee = 500
            };

            _fakeVehicleRepository.Setup(x => x.AddVehicle(It.IsAny<Vehicle>()))
                           .Throws(new Exception("Simulated Exception"));

            var result = await _vehicleService.AddVehicle(invalidVehicleToCreate);

            Assert.Null(result);
        }

        [Fact]
        public async void GetAllByOperatorId_HasOperator_ReturnsVehicleDtoList()
        {
            var expectedVehicleDtoList = new List<VehicleDto>
            {
                new VehicleDto
                {
                   Id = 1,
                   OperatorName = "Operator 1",
                   CRNumber = "CRNumber",
                   PlateNumber = "PlateNumber",
                   BodyType = "BodyType",
                   Make = "Make",
                   RentFee = 500,
                   RentStatus = true
                },
                new VehicleDto
                {
                    Id = 2,
                    OperatorName = "Operator 2",
                    CRNumber = "CRNumber",
                    PlateNumber = "PlateNumber",
                    BodyType = "BodyType",
                    Make = "Make",
                    RentFee = 500,
                    RentStatus = true
                }
            };
            _fakeVehicleRepository.Setup(x => x.GetAllByOperatorId(It.IsAny<int>()))
                                  .ReturnsAsync(It.IsAny<List<Vehicle>>());
                                  

            _fakeMapper.Setup(x => x.Map<IEnumerable<VehicleDto>>(It.IsAny<List<Vehicle>>()))
                       .Returns(expectedVehicleDtoList);

            var result = await _vehicleService.GetAllByOperatorId(1);

            Assert.NotNull(result);
            Assert.Equal(expectedVehicleDtoList, result);
        }
     
        [Fact]
        public async void GetAllByOperatorId_Exception_ReturnsNull()
        {          
            _fakeVehicleRepository.Setup(x => x.GetAllByOperatorId(1))
                                 .Throws(new Exception("Simulated Exception"));
        
            var result = await _vehicleService.GetAllByOperatorId(1);
        
            Assert.Null(result);
        }
        
        [Fact]
        public async void GetById_HasVehicle_ReturnsVehicleDto()
        {
            _fakeVehicleRepository.Setup(x => x.GetById(It.IsAny<int>()))
                                   .ReturnsAsync(new Vehicle());

            _fakeMapper.Setup(x => x.Map<VehicleDto>(It.IsAny<Vehicle>()))
                       .Returns(new VehicleDto());

            var result = await _vehicleService.GetById(1);

            Assert.NotNull(result);
            Assert.IsType<VehicleDto>(result);
        }
     
        [Fact]
        public async void GetById_NoDriver_ReturnsNull()
        {
            _fakeVehicleRepository.Setup(x => x.GetById(It.IsAny<int>()))
                                  .ReturnsAsync((Vehicle)null!);

            var result = await _vehicleService.GetById(It.IsAny<int>());

            Assert.Null(result);
        }
      
        [Fact]
        public async void GetById_Exception_ReturnsNull()
        {
          
            _fakeVehicleRepository.Setup(x => x.GetById(1))
                                 .Throws(new Exception("Simulated Exception"));
           
            var result = await _vehicleService.GetById(1);
            
            Assert.Null(result);
        }
       
        [Fact]
        public async void GetByPlateNumber_HasDriver_ReturnsVehicleDto()
        {
            _fakeVehicleRepository.Setup(x => x.GetByPlateNumber(It.IsAny<string>()))
                                  .ReturnsAsync(new Vehicle());

            _fakeMapper.Setup(x => x.Map<VehicleDto>(It.IsAny<Vehicle>()))
                       .Returns(new VehicleDto());

            var result = await _vehicleService.GetByPlateNumber(It.IsAny<string>());

            Assert.NotNull(result);
            Assert.IsType<VehicleDto>(result);
        }
       
        [Fact]
        public async void GetByPlateNumber_NoVehicle_ReturnsNull()
        {
            _fakeVehicleRepository.Setup(x => x.GetByPlateNumber(It.IsAny<string>()))
                                    .ReturnsAsync((Vehicle)null!); ;

            var result = await _vehicleService.GetByPlateNumber(It.IsAny<string>());

            Assert.Null(result);
        }
        
        [Fact]
        public async void GetByPlateNumber_Exception_ReturnsNull()
        {
            
            _fakeVehicleRepository.Setup(x => x.GetByPlateNumber("PlateNumber"))
                                 .Throws(new Exception("Simulated Exception"));
           
            var result = await _vehicleService.GetByPlateNumber("PlateNumber");
            
            Assert.Null(result);
        }
        
        [Fact]
        public async void UpdateRentStatus_HasVehicle_ReturnsVehicleDto()
        {
           
            int vehicleId = 1;
            bool rentStatus = true;

           
            var vehicleToUpdate = new Vehicle
            {
                Id = 1,
                OperatorName = "Operator",
                CRNumber = "CRNumber",
                PlateNumber = "PlateNumber",
                BodyType = "BodyType",
                Make = "Make",
                RentFee = 500,
                RentStatus = false
            };

            var expectedVehicleDto = new VehicleDto
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

            _fakeMapper.Setup(x => x.Map<Vehicle>(vehicleToUpdate))
                       .Returns(new Vehicle { Id = vehicleId });

            _fakeVehicleRepository.Setup(x => x.UpdateRentStatus(vehicleId, rentStatus))
                                   .ReturnsAsync(new Vehicle { Id = vehicleId });

            _fakeMapper.Setup(x => x.Map<VehicleDto>(It.IsAny<Vehicle>()))
                       .Returns(expectedVehicleDto);

            
            var result = await _vehicleService.UpdateRentStatus(vehicleId, rentStatus);

           
            Assert.Equal(expectedVehicleDto, result);
        }
       
        [Fact]
        public async void UpdateRentStatus_Exception_ReturnsNull()
        {
           
            int vehicleId = 1;
            bool rentStatus = true;

        
            var vehicleToUpdate = new Vehicle
            {
                Id = 1,
                OperatorName = "Operator",
                CRNumber = "CRNumber",
                PlateNumber = "PlateNumber",
                BodyType = "BodyType",
                Make = "Make",
                RentFee = 500,
                RentStatus = false
            };

            _fakeVehicleRepository.Setup(x => x.UpdateRentStatus(vehicleId, rentStatus))
                                   .ThrowsAsync(new System.Exception("Test Exception"));

            
            var result = await _vehicleService.UpdateRentStatus(vehicleId, rentStatus);

          
            Assert.Null(result);
        }
     
        [Fact]
        public async void UpdateRentFee_HasVehicle_ReturnsVehicleDto()
        {
           
            int vehicleId = 1;
            double rentFee = 1000;

            
            var vehicleToUpdate = new Vehicle
            {
                Id = 1,
                OperatorName = "Operator",
                CRNumber = "CRNumber",
                PlateNumber = "PlateNumber",
                BodyType = "BodyType",
                Make = "Make",
                RentFee = 500,
                RentStatus = false
            };

            var expectedVehicleDto = new VehicleDto
            {
                Id = 1,
                OperatorName = "Operator",
                CRNumber = "CRNumber",
                PlateNumber = "PlateNumber",
                BodyType = "BodyType",
                Make = "Make",
                RentFee = 1000,
                RentStatus = true
            };

            _fakeMapper.Setup(x => x.Map<Vehicle>(vehicleToUpdate))
                       .Returns(new Vehicle { Id = vehicleId });

            _fakeVehicleRepository.Setup(x => x.UpdateRentFee(vehicleId, rentFee))
                                   .ReturnsAsync(new Vehicle { Id = vehicleId });

            _fakeMapper.Setup(x => x.Map<VehicleDto>(It.IsAny<Vehicle>()))
                       .Returns(expectedVehicleDto);

          
            var result = await _vehicleService.UpdateRentFee(vehicleId, rentFee);

           
            Assert.Equal(expectedVehicleDto, result);
        }

     
        [Fact]
        public async void UpdateRentFee_Exception_ReturnsNull()
        {
           
            int vehicleId = 1;
            double rentFee = 1000;

          
            var vehicleToUpdate = new Vehicle
            {
                Id = 1,
                OperatorName = "Operator",
                CRNumber = "CRNumber",
                PlateNumber = "PlateNumber",
                BodyType = "BodyType",
                Make = "Make",
                RentFee = 500,
                RentStatus = false
            };

            _fakeVehicleRepository.Setup(x => x.UpdateRentFee(vehicleId, rentFee))
                                   .ThrowsAsync(new System.Exception("Test Exception"));

          
            var result = await _vehicleService.UpdateRentFee(vehicleId, rentFee);

            
            Assert.Null(result);
        }
       
        [Fact]
        public async void DeleteVehicle_HasVehicle_ReturnsTrue()
        {
            
            int vehicleId = 1;

            _fakeVehicleRepository.Setup(x => x.DeleteVehicle(vehicleId))
                           .ReturnsAsync(true);

         
            var result = await _vehicleService.DeleteVehicle(vehicleId);

         
            Assert.True(result);
        }
     
        [Fact]
        public async void DeleteVehicle_Exception_ReturnsFalse()
        {
           
            int vehicleId = 1;
            _fakeVehicleRepository.Setup(x => x.DeleteVehicle(vehicleId))
                           .ThrowsAsync(new System.Exception("Test Exception"));
        
            var result = await _vehicleService.DeleteVehicle(vehicleId);
       
            Assert.False(result);
        }
    }
}
