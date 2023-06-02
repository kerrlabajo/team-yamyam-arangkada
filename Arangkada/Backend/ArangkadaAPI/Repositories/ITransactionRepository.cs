using ArangkadaAPI.Models;

namespace ArangkadaAPI.Repositories
{
    public interface ITransactionRepository
    {
        Task<int> CreateTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllByOperatorId(int operatorId);
        Task<Transaction> GetById(int id);
        Task<Transaction> UpdateTransaction(int id, Transaction transaction);
        Task<bool> DeleteTransactionById(int id);
    }
}
