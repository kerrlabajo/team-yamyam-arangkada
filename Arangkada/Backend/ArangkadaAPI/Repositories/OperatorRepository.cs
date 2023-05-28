using ArangkadaAPI.Context;
using ArangkadaAPI.Models;
using Dapper;
namespace ArangkadaAPI.Repositories
{
    public class OperatorRepository : IOperatorRepository
    {
        private readonly DapperContext _context;

        public OperatorRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<string?> GenerateVerificationCode()
        {
            string formattedCode = await Task.Run(() =>
            {
                Random random = new Random();
                int verificationCode = random.Next(10000);
                return verificationCode.ToString("D4");
            });

            return formattedCode;
        }

        public async Task<int> CreateOperator(Operator op)
        {
            var verificationCode = await GenerateVerificationCode();

            var sql = "INSERT Operator ([FullName], [Username], [Password], [Email], [VerificationCode]) " +
                      "VALUES (@FullName, @Username, @Password, @Email, @VerificationCode); " +
                      "SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new
                {
                    op.FullName,
                    op.Username,
                    op.Password,
                    op.Email,
                    VerificationCode = verificationCode
                });
            }
        }

        public async Task<IEnumerable<Operator>> GetAll()
        {
            var sql = "SELECT o.*, " +
                      "(SELECT COUNT(*) FROM Vehicle v WHERE v.OperatorId = o.Id) AS Vehicles, " +
                      "(SELECT COUNT(*) FROM Driver d WHERE d.OperatorId = o.Id) AS Drivers " +
                      "FROM Operator o;";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<Operator>(sql);
            }
        }

        public async Task<Operator> GetById(int id)
        {
            var sql = "SELECT o.*, " +
                      "(SELECT COUNT(*) FROM Vehicle v WHERE v.OperatorId = o.Id) AS Vehicles, " +
                      "(SELECT COUNT(*) FROM Driver d WHERE d.OperatorId = o.Id) AS Drivers " +
                      "FROM Operator o " +
                      "WHERE o.Id = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Operator>(sql, new { Id = id });
            }
        }

        public async Task<Operator> GetByFullName(string? fullName)
        {
            var sql = "SELECT o.*, " +
                      "(SELECT COUNT(*) FROM Vehicle v WHERE v.OperatorId = o.Id) AS Vehicles, " +
                      "(SELECT COUNT(*) FROM Driver d WHERE d.OperatorId = o.Id) AS Drivers " +
                      "FROM Operator o " +
                      "WHERE o.FullName = @FullName;";
            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Operator>(sql, new { FullName = fullName });
            }
        }

        public async Task<Operator> GetByUsername(string? username)
        {
            var sql = "SELECT o.*, " +
                      "(SELECT COUNT(*) FROM Vehicle v WHERE v.OperatorId = o.Id) AS Vehicles, " +
                      "(SELECT COUNT(*) FROM Driver d WHERE d.OperatorId = o.Id) AS Drivers " +
                      "FROM Operator o " +
                      "WHERE o.Username = @Username;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Operator>(sql, new { Username = username });
            }
        }

        public async Task<string> GetPasswordById(int id)
        {
            var sql = "SELECT o.Password " +
                      "FROM Operator o " +
                      "WHERE o.Id = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<string>(sql, new { Id = id });
            }
        }

        public async Task<bool> GetIsVerifiedById(int id)
        {
            var sql = "SELECT o.IsVerified " +
                      "FROM Operator o " +
                      "WHERE o.Id = @Id;";
            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<bool>(sql, new { Id = id });
            }
        }

        public async Task<Operator> UpdateOperator(int id, Operator op)
        {
            var sql = "UPDATE Operator SET [FullName] = @FullName, [Username] = @Username, " +
                "[Password] = @Password, [Email] = @Email WHERE [Id] = @Id;";

            using (var con = _context.CreateConnection())
            {
                await con.ExecuteScalarAsync(
                    sql,
                    new
                    {
                        id,
                        op.FullName,
                        op.Username,
                        op.Password,
                        op.Email
                    }
                   );
                {
                    return await GetById(id);
                }
            }
        }

        public async Task<Operator> UpdateIsVerified(int id, bool isVerified)
        {
            var sql = "UPDATE Operator SET [IsVerified] = @IsVerified WHERE [Id] = @Id;";
            using (var con = _context.CreateConnection())
            {
                await con.ExecuteScalarAsync(
                    sql,
                    new
                    {
                        Id = id,
                        IsVerified = isVerified
                    }
                   );
                {
                    return await GetById(id);
                }
            }
        }

        public async Task<bool> DeleteOperator(int id)
        {
            await DeleteDataByOperatorId(id);

            var sql = "DELETE FROM Operator WHERE [Id] = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteAsync(sql, new { Id = id }) > 0;
            }
        }

        private async Task<bool> DeleteDataByOperatorId(int operatorId)
        {
            var sql = "DELETE FROM [dbo].[Transaction] WHERE [OperatorId] = @OperatorId; " +
                      "DELETE FROM [dbo].[Driver] WHERE [OperatorId] = @OperatorId; " +
                      "DELETE FROM [dbo].[Vehicle] WHERE [OperatorId] = @OperatorId;";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteAsync(sql, new { OperatorId = operatorId }) > 0;
            }
        }
    }
}
