using ArangkadaAPI.Models;
using ArangkadaAPI.Dtos.Operator;

namespace ArangkadaAPI.Services
{

    /// <summary>
    /// Represents the service interface for managing operators.
    /// </summary>
    public interface IOperatorService
    {
        /// <summary>
        /// Creates a new operator.
        /// </summary>
        /// <param name="OperatorToCreate">The operator data to create.</param>
        /// <returns>The created operator DTO, or null if creation fails.</returns>
        Task<OperatorDto?> CreateOperator(OperatorCreationDto OperatorToCreate);

        /// <summary>
        /// Retrieves an operator by ID.
        /// </summary>
        /// <param name="id">The ID of the operator.</param>
        /// <returns>The operator DTO matching the ID, or null if not found.</returns>
        Task<OperatorDto?> GetById(int id);

        /// <summary>
        /// Retrieves an operator by full name.
        /// </summary>
        /// <param name="fullName">The full name of the operator.</param>
        /// <returns>The operator DTO matching the full name, or null if not found.</returns>
        Task<OperatorDto?> GetByFullName(string? fullName);

        /// <summary>
        /// Retrieves an operator by username.
        /// </summary>
        /// <param name="username">The username of the operator.</param>
        /// <returns>The operator DTO matching the username, or null if not found.</returns>
        Task<OperatorDto?> GetByUsername(string? username);

        /// <summary>
        /// Retrieves the verification status of an operator by ID.
        /// </summary>
        /// <param name="id">The ID of the operator.</param>
        /// <returns>True if the operator is verified, false if not verified, or null if not found.</returns>
        Task<bool?> GetIsVerifiedById(int id);

        /// <summary>
        /// Retrieves the password of an operator by ID.
        /// </summary>
        /// <param name="id">The ID of the operator.</param>
        /// <returns>The password of the operator, or null if not found.</returns>
        Task<string?> GetPasswordById(int id);

        /// <summary>
        /// Updates an existing operator.
        /// </summary>
        /// <param name="id">The ID of the operator to update.</param>
        /// <param name="OperatorToUpdate">The updated operator data.</param>
        /// <returns>The updated operator DTO, or null if update fails or operator not found.</returns>
        Task<OperatorDto?> UpdateOperator(int id, OperatorUpdateDto OperatorToUpdate);

        /// <summary>
        /// Updates the verification status of an operator.
        /// </summary>
        /// <param name="id">The ID of the operator to update.</param>
        /// <param name="isVerified">The verification status to set.</param>
        /// <returns>The updated operator DTO, or null if update fails or operator not found.</returns>
        Task<OperatorDto?> UpdateIsVerified(int id, bool isVerified);

        /// <summary>
        /// Deletes an operator.
        /// </summary>
        /// <param name="id">The ID of the operator to delete.</param>
        /// <returns>True if the operator is successfully deleted, false otherwise.</returns>
        Task<bool> DeleteOperator(int id);
    }
}
