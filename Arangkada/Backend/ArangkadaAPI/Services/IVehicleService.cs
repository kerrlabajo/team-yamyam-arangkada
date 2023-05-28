using ArangkadaAPI.Dtos.Vehicle;

namespace ArangkadaAPI.Services
{
    /// <summary>
    /// Interface for Vehicle service
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Adds a new vehicle
        /// </summary>
        /// <param name="vehicleToAdd">The vehicle to be added</param>
        /// <returns>Newly added Vehicle, null if invalid</returns>
        Task<VehicleDto?> AddVehicle(VehicleCreationDto vehicleToAdd);
        /// <summary>
        /// Retrieves all currently registered vehicles in the system
        /// </summary>
        /// <returns>A list of Vehicles</returns>
        Task<IEnumerable<VehicleDto>?> GetAll();
        /// <summary>
        /// Retrieves all currently registered vehicles whose operatorId is equal to the parameter
        /// </summary>
        /// <param name="operatorId">The id of the operator</param>
        /// <returns>A list of vehicles with matching operator id's</returns>
        Task<IEnumerable<VehicleDto>?> GetAllByOperatorId(int operatorId);
        /// <summary>
        /// Retrieves a single vehicle from the system whose id is equal to the parameter
        /// </summary>
        /// <param name="id">The id of the vehicle to retrieve</param>
        /// <returns>A vehicle with matching id</returns>
        Task<VehicleDto?> GetById(int id);
        /// <summary>
        /// Retrieves a single vehicle from the system whose platenumber is equal to the parameter
        /// </summary>
        /// <param name="plateNumber">The plate number of the vehicle to retrieve</param>
        /// <returns>A vehicle with matching plate number</returns>
        Task<VehicleDto?> GetByPlateNumber(string? plateNumber);
        /// <summary>
        /// Updates a vehicle's rent status
        /// </summary>
        /// <param name="id">The id of the vehicle to update</param>
        /// <param name="status">New vehicle status</param>
        /// <returns>A vehicle with updated rent status</returns>
        Task<VehicleDto?> UpdateRentStatus(int id, bool status);
        /// <summary>
        /// Updates a vehicle's rent fee
        /// </summary>
        /// <param name="id">The id of the vehicle to update</param>
        /// <param name="rentFee">New vehicle rent fee</param>
        /// <returns>A vehicle with updated rent fee</returns>
        Task<VehicleDto?> UpdateRentFee(int id, double rentFee);
        /// <summary>
        /// Deletes a vehicle from the system
        /// </summary>
        /// <param name="id">The id of the vehicle to be deleted</param>
        /// <returns>A message on whether or not the vehicle was successfully deleted</returns>
        Task<bool> DeleteVehicle(int id);
    }
}
