using ArangkadaAPI.Dtos.Transaction;
using ArangkadaAPI.Mappings;
using ArangkadaAPI.Models;
using AutoMapper;

namespace ArangkadaTests.MappingsTests
{
    public class TransactionMappingsTest
    {
        private readonly IMapper _mapper;

        public TransactionMappingsTest()
        {
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new TransactionMappings()));
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public void TransactionMappings_ValidConfiguration()
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
        public void CreateMap_TransactionToTransactionDto_ValidMapping()
        {
            var transaction = new Transaction
            {
                Id = 1,
                OperatorName = "Operator 1",
                DriverName = "John Smith",
                Amount = 1000,
                Date = "2023-05-01"
            };

            var result = _mapper.Map<TransactionDto>(transaction);

            Assert.Equal(transaction.Id, result.Id);
            Assert.Equal(transaction.OperatorName, result.OperatorName);
            Assert.Equal(transaction.DriverName, result.DriverName);
            Assert.Equal(transaction.Amount, result.Amount);
            Assert.Equal(transaction.Date, result.Date);
        }

        [Fact]
        public void CreateMap_TransactionUpdateDtoToTransaction_ValidMapping()
        {
            var transactionUpdateDto = new TransactionUpdateDto
            {
                Amount = 1000,
                Date = "2023-05-01"
            };

            var expectedTransaction = new Transaction
            {
                Amount = 1000,
                Date = "2023-05-01"
            };
            
            var result = _mapper.Map<Transaction>(transactionUpdateDto);

            Assert.Equal(expectedTransaction.Amount, result.Amount);
            Assert.Equal(expectedTransaction.Date, result.Date);
        }

        [Fact]
        public void CreateMap_TransactionCreationDtoToTransaction_ValidMapping()
        {
            var transactionCreationDto = new TransactionCreationDto
            {
                OperatorName = "Operator 1",
                DriverName = "John Smith",
                Amount = 1000,
                Date = "2023-05-01"
            };
            var expectedTransaction = new Transaction
            {
                OperatorName = "Operator 1",
                DriverName = "John Smith",
                Amount = 1000,
                Date = "2023-05-01"
            };

            var result = _mapper.Map<Transaction>(transactionCreationDto);

            Assert.Equal(expectedTransaction.OperatorName, result.OperatorName);
            Assert.Equal(expectedTransaction.DriverName, result.DriverName);
            Assert.Equal(expectedTransaction.Amount, result.Amount);
            Assert.Equal(expectedTransaction.Date, result.Date);
        }
    }
}
