using ArangkadaAPI.Dtos.Transaction;
using ArangkadaAPI.Models;
using ArangkadaAPI.Repositories;
using AutoMapper;
using System.Diagnostics;

namespace ArangkadaAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;
        public TransactionService(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TransactionDto?> CreateTransaction(TransactionCreationDto transactionToCreate)
        {
            try
            {
                // Convert Models to DTO
                var transacModel = _mapper.Map<Transaction>(transactionToCreate);
                transacModel.Id = await _repository.CreateTransaction(transacModel);
                var transacDto = _mapper.Map<TransactionDto>(transacModel);
                return transacDto;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }

        }

        public async Task<IEnumerable<TransactionDto>?> GetAll()
        {
            try
            {
                var transacModels = await _repository.GetAll();
                return _mapper.Map<IEnumerable<TransactionDto>>(transacModels);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<TransactionDto>?> GetAllByOperatorId(int operatorId)
        {
            try
            {
                var transacModels = await _repository.GetAllByOperatorId(operatorId);

                if (transacModels == null)
                {
                    return null;
                }

                return _mapper.Map<IEnumerable<TransactionDto>>(transacModels);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<TransactionDto>?> GetAllByDriverId(int driverId)
        {
            try
            {
                var transacModels = await _repository.GetAllByDriverId(driverId);

                if (transacModels == null)
                {
                    return null;
                }

                return _mapper.Map<IEnumerable<TransactionDto>>(transacModels);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<TransactionDto?> GetById(int id)
        {
            try
            {
                var transacModel = await _repository.GetById(id);
            
                if (transacModel == null)
                {
                    return null;
                }

                return _mapper.Map<TransactionDto?>(transacModel);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<TransactionDto?> UpdateTransaction(int id, TransactionUpdateDto transactionToUpdate)
        {
            try
            {
                var transacModel = _mapper.Map<Transaction>(transactionToUpdate);
                transacModel.Id = id;
                var updatedTransaction = await _repository.UpdateTransaction(id, transacModel);
                return _mapper.Map<TransactionDto>(updatedTransaction);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<bool> DeleteTransactionById(int id)
        {
            try
            {
                return await _repository.DeleteTransactionById(id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}
