using ArangkadaAPI.Dtos.Transaction;
using ArangkadaAPI.Models;
using ArangkadaAPI.Repositories;
using ArangkadaAPI.Services;
using AutoMapper;
using Moq;

namespace ArangkadaTests.ServicesTests
{
    public class ITransactionServicesTest
    {
        private readonly ITransactionService _transactionService;
        private readonly Mock<ITransactionRepository> _fakeTransactionRepository;
        private readonly Mock<IMapper> _fakeMapper;

        public ITransactionServicesTest()
        {
            _fakeTransactionRepository = new Mock<ITransactionRepository>();
            _fakeMapper = new Mock<IMapper>();
            _transactionService = new TransactionService(_fakeTransactionRepository.Object, _fakeMapper.Object);
        }

        [Fact]
        public async void CreateTransaction_ValidTransactionToCreate_ReturnsTransactionDto()
        {
            _fakeMapper.Setup(m => m.Map<Transaction>(It.IsAny<TransactionCreationDto>()))
                        .Returns(new Transaction());

            _fakeTransactionRepository.Setup(r => r.CreateTransaction(It.IsAny<Transaction>()))
                                      .ReturnsAsync(1);

            _fakeMapper.Setup(m => m.Map<TransactionDto>(It.IsAny<Transaction>())).Returns(new TransactionDto());

            var result = await _transactionService.CreateTransaction(It.IsAny<TransactionCreationDto>());

            Assert.NotNull(result);
            Assert.IsType<TransactionDto>(result);
        }

        [Fact]
        public async void CreateTransaction_Exception_ReturnsNull()
        {
            var transactionCreationDto = new TransactionCreationDto
            {
                OperatorName = "Operator 1",
                DriverName = null,
                Amount = 1000,
                Date = "2023-05-10"
            };

            _fakeMapper.Setup(m => m.Map<Transaction>(transactionCreationDto))
                       .Throws(new Exception("Simulated Exception"));

            var result = await _transactionService.CreateTransaction(transactionCreationDto);

            Assert.Null(result);
        }

        [Fact]
        public async void GetAllByOperatorId_HasOperatorId_ReturnsTransactionDtoList()
        { 
            var transactionModelList = new List<Transaction>
            {
                new Transaction
                {
                    Id = 1,
                    OperatorName = "Operator 1",
                    DriverName = "John Smith",
                    Amount = 1000,
                    Date = "2023-05-10"
                },
                new Transaction
                {
                    Id = 1,
                    OperatorName = "Operator 1",
                    DriverName = "John Doe",
                    Amount = 2000,
                    Date = "2023-05-11"
                }
            };

            var expectedTransactionDtoList = new List<TransactionDto>
            {
                new TransactionDto
                {
                    Id = 1,
                    OperatorName = "Operator 1",
                    DriverName = "John Smith",
                    Amount = 1000,
                    Date = "2023-05-10"
                },
                new TransactionDto
                {
                    Id = 1,
                    OperatorName = "Operator 1",
                    DriverName = "John Doe",
                    Amount = 2000,
                    Date = "2023-05-11"
                }
            };

            _fakeTransactionRepository.Setup(r => r.GetAllByOperatorId(1))
                                      .ReturnsAsync(transactionModelList);

            _fakeMapper.Setup(m => m.Map<IEnumerable<TransactionDto>>(transactionModelList))
                       .Returns(expectedTransactionDtoList);

            var result = await _transactionService.GetAllByOperatorId(1);

            Assert.NotNull(result);
            Assert.Equal(expectedTransactionDtoList, result);
        }

        [Fact]
        public async void GetAllByOperatorID_Exception_ReturnsNull()
        {
            _fakeTransactionRepository.Setup(r => r.GetAllByOperatorId(It.IsAny<int>())).ThrowsAsync(new Exception("Test exception"));

            var result = await _transactionService.GetAllByOperatorId(It.IsAny<int>());

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_HasTransaction_ReturnsTransactionDto()
        { 
            _fakeTransactionRepository.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync(new Transaction());
            _fakeMapper.Setup(m => m.Map<TransactionDto>(It.IsAny<Transaction>())).Returns(new TransactionDto());

            var result = await _transactionService.GetById(1);

            Assert.NotNull(result);
            Assert.IsType<TransactionDto>(result);
        }

        [Fact]
        public async void GetById_NoTransaction_ReturnsNull()
        {
            _fakeTransactionRepository.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Transaction)null!);

            var result = await _transactionService.GetById(It.IsAny<int>());

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_Exception_ReturnsNull()
        {
            _fakeTransactionRepository.Setup(r => r.GetById(It.IsAny<int>())).ThrowsAsync(new Exception("Test exception"));

            var result = await _transactionService.GetById(It.IsAny<int>());

            Assert.Null(result);
        }

        [Fact]
        public async void UpdateTransaction_HasTransaction_ReturnsTransactionDto()
        {
            int id = 1;

            var transactionToUpdate = new TransactionUpdateDto
            {
                Amount = 1000,
                Date = "2023-05-10"
            };

            var expectedTransactionDto = new TransactionDto
            {
                Id = id,
                OperatorName = "Operator 1",
                DriverName = "John Smith",
                Amount = 1000,
                Date = "2023-05-10"
            };

            _fakeMapper.Setup(m => m.Map<Transaction>(transactionToUpdate))
                       .Returns(new Transaction { Id = 1});

            _fakeTransactionRepository.Setup(r => r.UpdateTransaction(id, It.IsAny<Transaction>()))
                                      .ReturnsAsync(new Transaction { Id = 1 });

            _fakeMapper.Setup(m => m.Map<TransactionDto>(It.IsAny<Transaction>()))
                       .Returns(expectedTransactionDto);

            var result = await _transactionService.UpdateTransaction(id, transactionToUpdate);

            Assert.Equal(expectedTransactionDto, result);
        }

        [Fact]
        public async void UpdateTransaction_Exception_ReturnsNull()
        {
            int id = 1;
            var transactionToUpdate = new TransactionUpdateDto
            {
                Amount = 1000,
                Date = "2023-05-10"
            };
            _fakeMapper.Setup(m => m.Map<Transaction>(transactionToUpdate))
                       .Throws(new Exception("Test exception"));

            var result = await _transactionService.UpdateTransaction(id, transactionToUpdate);

            Assert.Null(result);
        }

        [Fact]
        public async void DeleteTransaction_HasTransaction_ReturnsTrue()
        {
            int id = 1;
            _fakeTransactionRepository.Setup(r => r.DeleteTransactionById(id))
                                      .ReturnsAsync(true);

            var result = await _transactionService.DeleteTransactionById(id);

            Assert.True(result);
        }

        [Fact]
        public async void DeleteTransaction_Exception_ReturnsFalse()
        {
            int id = 1;
            _fakeTransactionRepository.Setup(r => r.DeleteTransactionById(id))
                                      .ThrowsAsync(new Exception("Test exception"));

            var result = await _transactionService.DeleteTransactionById(id);

            Assert.False(result);
        }


    }
}
