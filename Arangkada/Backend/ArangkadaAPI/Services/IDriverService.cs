using ArangkadaAPI.Dtos.Driver;

namespace ArangkadaAPI.Services
{
    /// <summary>
    /// Represents the service interface for managing drivers.
    /// </summary>
    public interface IDriverService
    {
        /// <summary>
        /// Creates a new driver.
        /// </summary>
        /// <param name="DrivertoCreate">The driver data to create.</param>
        /// <returns>The created driver DTO, or null if creation fails.</returns>
        Task<DriverDto?> CreateDriver(DriverCreationDto DrivertoCreate);

        /// <summary>
        /// Retrieves all drivers.
        /// </summary>
        /// <returns>A collection of driver DTOs, or null if no operators exist.</returns>
        Task<IEnumerable<DriverDto>?> GetAll();

        /// <summary>
        /// Retrieves all drivers by Operator Id.
        /// </summary>
        /// <param name="operatorId">The driver data that need to get</param>
        /// <returns> A collection of driver DTOs, or null if no operators exist. </returns>
        Task<IEnumerable<DriverDto>?> GetAllByOperatorId(int operatorId);

        /// <summary>
        /// Retrieves a Driver by ID.
        /// </summary>
        /// <param name="id">The ID of the Driver.</param>
        /// <returns>The driver DTO matching the ID, or null if not found.</returns>
        Task<DriverDto?> GetById(int id);

        /// <summary>
        /// Retrieves a driver by his/her full name.
        /// </summary>
        /// <param name="fullName">The full name of the driver.</param>
        /// <returns>The driver DTO matching the full name, or null if not found.</returns>
        Task<DriverDto?> GetByFullName(string? fullName);

        /// <summary>
        /// Updates an existing driver.
        /// </summary>
        /// <param name="id">The ID of the driver to update.</param>
        /// <param name="DrivertoUpdate">The updated operator data.</param>
        /// <returns>The updated driver DTO, or null if update fails or operator not found.</returns>
        Task<DriverDto?> UpdateDriver(int id, DriverUpdateDto DrivertoUpdate);

        /// <summary>
        /// Updates the assignedVehicle of a driver.
        /// </summary>
        /// <param name="id">The ID of the driver to update.</param>
        /// <param name="plateNumberToAssign">The platenumber that is assign for the driver</param>
        /// <returns>The updated driver DTO, or null if update fails or operator not found</returns>
        Task<DriverDto?> UpdateVehicleAssigned(int id, string? plateNumberToAssign);

        /// <summary>
        /// Deletes a driver.
        /// </summary>
        /// <param name="id">The ID of the driver to delete.</param>
        /// <returns>True if the driver is successfully deleted, false otherwise.</returns>
        Task<bool> DeleteDriver(int id);
    }
}
