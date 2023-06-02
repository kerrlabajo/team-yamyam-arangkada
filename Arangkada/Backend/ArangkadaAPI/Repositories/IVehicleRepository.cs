using ArangkadaAPI.Models;

namespace ArangkadaAPI.Repositories
{
    public interface IVehicleRepository
    {
        Task<int> AddVehicle(Vehicle vehicle);
        Task<IEnumerable<Vehicle>> GetAllByOperatorId(int operatorId);
        Task<Vehicle> GetById(int id);
        Task<Vehicle> GetByPlateNumber(string? plateNumber);
        Task<Vehicle> UpdateRentStatus(int id, bool vehicle);
        Task<Vehicle> UpdateRentFee(int id, double vehicle);
        Task<bool> DeleteVehicle(int id);
    }
}