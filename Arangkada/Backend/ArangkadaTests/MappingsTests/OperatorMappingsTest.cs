using ArangkadaAPI.Dtos.Operator;
using ArangkadaAPI.Mappings;
using ArangkadaAPI.Models;
using AutoMapper;

namespace ArangkadaTests.MappingsTests
{
    public class OperatorMappingsTest
    {
        private readonly IMapper _mapper;

        public OperatorMappingsTest()
        {
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new OperatorMappings()));

            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public void OperatorMappings_ValidConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OperatorMappings>();
            });

            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void CreateMap_OperatorToOperatorDto_ValidMapping()
        {
            var op = new Operator
            {
                Id = 1,
                FullName = "Hayley Williams",
                Username = "hayleywilliams",
                Password = "p4r4m0re",
                Email = "hayley.paramore@email.com",
                IsVerified = false,
                Vehicles = 0,
                Drivers = 0
            };

            var operatorDto = _mapper.Map<OperatorDto>(op);

            Assert.Equal(op.Id, operatorDto.Id);
            Assert.Equal(op.FullName, operatorDto.FullName);
            Assert.Equal(op.Username, operatorDto.Username);
            Assert.Equal(op.Email, operatorDto.Email);
            Assert.Equal(op.IsVerified, operatorDto.IsVerified);
            Assert.Equal(op.Vehicles, operatorDto.Vehicles);
            Assert.Equal(op.Drivers, operatorDto.Drivers);
        }

        [Fact]
        public void CreateMap_OperatorCreationDtoToOperator_ValidMapping()
        {
            var operatorCreationDto = new OperatorCreationDto
            {
                FullName = "Hayley Williams",
                Username = "hayleywilliams",
                Password = "p4r4m0re",
                Email = "hayley.paramore@email.com"
            };

            var op = _mapper.Map<Operator>(operatorCreationDto);

            Assert.Equal(operatorCreationDto.FullName, op.FullName);
            Assert.Equal(operatorCreationDto.Username, op.Username);
            Assert.Equal(operatorCreationDto.Password, op.Password);
            Assert.Equal(operatorCreationDto.Email, op.Email);
        }

        [Fact]
        public void CreateMap_OperatorUpdateDtoToOperator_ValidMapping()
        {
            var operatorUpdateDto = new OperatorUpdateDto
            {
                FullName = "Hayley Williams",
                Username = "hayleywilliams",
                Password = "p4r4m0re",
                Email = "hayley.paramore@email.com"
            };

            var op = _mapper.Map<Operator>(operatorUpdateDto);

            Assert.Equal(operatorUpdateDto.FullName, op.FullName);
            Assert.Equal(operatorUpdateDto.Username, op.Username);
            Assert.Equal(operatorUpdateDto.Password, op.Password);
            Assert.Equal(operatorUpdateDto.Email, op.Email);
        }
    }
}