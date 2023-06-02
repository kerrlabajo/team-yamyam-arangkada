using Dapper;
using ArangkadaAPI.Context;
using ArangkadaAPI.Models;

namespace ArangkadaAPI.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DapperContext _context;
        public TransactionRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateTransaction(Transaction transaction)
        {
            var sql = "INSERT [dbo].[Transaction] (Amount, Date, OperatorId, DriverId) " +
                     "VALUES (@Amount, @Date, @OperatorId, @DriverId) " +
                     "SELECT CAST(SCOPE_IDENTITY() as int);";

            var driverId = await GetDriverId(transaction.DriverName);
            var operatorId = await GetOperatorId(transaction.OperatorName);

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(
                    sql, new
                    {
                        transaction.Amount,
                        transaction.Date,
                        OperatorId = operatorId,
                        DriverId = driverId
                    });
            }
        }

        public async Task<IEnumerable<Transaction>> GetAllByOperatorId(int operatorId)
        {
            var sql = "SELECT t.*, o.FullName AS OperatorName, d.FullName AS DriverName " +
                      "FROM [dbo].[Transaction] t " +
                      "JOIN Operator o ON t.OperatorId = o.Id " +
                      "JOIN Driver d ON t.DriverId = d.Id " +
                      "WHERE o.Id = @OperatorId";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<Transaction>(sql, new { OperatorId = operatorId });
            }
        }

        public async Task<Transaction> GetById(int id)
        {
            var sql = "SELECT t.*, o.FullName AS OperatorName, d.FullName AS DriverName " +
                      "FROM [dbo].[Transaction] t " +
                      "JOIN Operator o ON t.OperatorId = o.Id " +
                      "JOIN Driver d ON t.DriverId = d.Id " +
                      "WHERE t.Id = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Transaction>(sql, new { Id = id });
            }
        }

        public async Task<Transaction> UpdateTransaction(int id, Transaction transaction)
        {
            var sql = "UPDATE [dbo].[Transaction] " +
                      "SET Amount = @Amount, Date = @Date " +
                      "WHERE Id = @Id";
            var driverId = await GetDriverId(transaction.DriverName);
            var operatorId = await GetOperatorId(transaction.OperatorName);
            using (var con = _context.CreateConnection())
            {
                await con.ExecuteAsync(
                     sql, new
                     {
                        Id = id,
                        transaction.Amount,
                        transaction.Date
                    });
                return await GetById(id);
            }
        }

        public async Task<bool> DeleteTransactionById(int id)
        {
            var sql = "DELETE FROM [dbo].[Transaction] WHERE Id = @id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteAsync(sql, new {id }) > 0;
            }
        }

        private async Task<int> GetDriverId(string? driverName)
        {
            var sql_driverId = "SELECT DISTINCT Id FROM Driver WHERE FullName = @DriverName";
           
            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql_driverId, new { DriverName = driverName });
            }
        }

        private async Task<int> GetOperatorId(string? operatorName)
        {
            var sql_operatorId = "SELECT DISTINCT Id From Operator WHERE FullName = @OperatorName";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql_operatorId, new { OperatorName = operatorName });
            }
        }
    }
}
