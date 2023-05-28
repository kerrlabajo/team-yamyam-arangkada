using ArangkadaAPI.Models;

namespace ArangkadaAPI.Repositories
{
    public interface IDriverRepository
    {
        Task<int> CreateDriver(Driver driver);
        Task<IEnumerable<Driver>> GetAll();
        Task<IEnumerable<Driver>> GetAllByOperatorId(int operatorId);
        Task<Driver> GetById(int id);
        Task<Driver> GetByFullName(string? fullName);
        Task<Driver> UpdateDriver(int id, Driver driver);
        Task<Driver> UpdateVehicleAssigned(int id, string? plateNumberAssigned);
        Task<bool> DeleteDriver(int id);
    }
}
