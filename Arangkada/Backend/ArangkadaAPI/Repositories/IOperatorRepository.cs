using ArangkadaAPI.Models;

namespace ArangkadaAPI.Repositories
{
    public interface IOperatorRepository
    {
        Task<int> CreateOperator(Operator op);
        Task<Operator> GetById(int id);
        Task<Operator> GetByFullName(string? fullName);
        Task<Operator> GetByUsername(string? username);
        Task<string> GetPasswordById(int id);
        Task<bool> GetIsVerifiedById(int id);
        Task<Operator> UpdateOperator(int id, Operator op);
        Task<Operator> UpdateIsVerified(int id, bool isVerified);
        Task<bool> DeleteOperator(int id);
    }
}
