using ArangkadaAPI.Dtos.Transaction;

namespace ArangkadaAPI.Services
{
    /// <summary>
    /// Represents the service interface for managing transactions.
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Creates a new transaction.
        /// </summary>
        /// <param name="transactionToCreate"> Transaction Details to create.</param>
        /// <returns>
        /// The newly created transaction Dto if creation successful.
        /// Null if creation fails.
        /// </returns>
        Task<TransactionDto?> CreateTransaction(TransactionCreationDto transactionToCreate);

        /// <summary>
        /// Retrieves all transactions.
        /// </summary>
        /// <returns>
        /// A list of transaction Dto if retrieval successful.
        /// Null if no transactions exist.
        /// </returns>
        Task<IEnumerable<TransactionDto>?> GetAll();

        /// <summary>
        /// Retrieves all transactions by operator ID.
        /// </summary>
        /// <param name="operatorId">Id of the operator to find in transactions</param>
        /// <returns>
        /// A list of transaction Dto with the same Operator if retrieval successful.
        /// Null if no transactions exist with the given Operator.
        /// </returns>
        Task<IEnumerable<TransactionDto>?> GetAllByOperatorId(int operatorId);

        /// <summary>
        /// Retrieves all transactions by driver ID.
        /// </summary>
        /// <param name="driverId">Id of the driver to find in transactions</param>
        /// <returns>
        /// A list of transaction Dto with the same Driver if retrieval successful.
        /// Null if no transactions exist with the given Driver.
        /// </returns>
        Task<IEnumerable<TransactionDto>?> GetAllByDriverId(int driverId);

        /// <summary>
        /// Retrieves transaction by ID
        /// </summary>
        /// <param name="id">The ID of the transaction to retrieve</param>
        /// <returns>
        /// A transaction Dto with the same ID if retrieval successful.
        /// Null if no transaction exist with the given ID.
        /// </returns>
        Task<TransactionDto?> GetById(int id);

        /// <summary>
        /// Updates an existing transaction by Id.
        /// </summary>
        /// <param name="id">The ID of the transaction to update.</param>
        /// <param name="transactionToUpdate">The updated details of the transaction</param>
        /// <returns>
        /// The updated transaction Dto if update successful.
        /// Null if update fails or transaction not found.
        /// </returns>
        Task<TransactionDto?> UpdateTransaction(int id, TransactionUpdateDto transactionToUpdate);

        /// <summary>
        /// Deletes an existing transaction by Id.
        /// </summary>
        /// <param name="id">The ID of the transaction to delete.</param>
        /// <returns>
        /// True if deletion successful.
        /// False if deletion fails or transaction not found.
        /// </returns>
        Task<bool> DeleteTransactionById(int id);
        

    }
}
