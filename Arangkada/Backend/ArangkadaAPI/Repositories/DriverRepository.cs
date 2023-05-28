using ArangkadaAPI.Context;
using ArangkadaAPI.Models;
using Dapper;
using Microsoft.IdentityModel.Tokens;

namespace ArangkadaAPI.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly DapperContext _context;

        public DriverRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateDriver(Driver driver)
        {
            var sql = "INSERT Driver (FullName, Address, ContactNumber, LicenseNumber, ExpirationDate, DLCodes, OperatorId) " +
                      "VALUES (@FullName, @Address, @ContactNumber, @LicenseNumber, @ExpirationDate, @DLCodes, @OperatorId) " +
                      "SELECT CAST(SCOPE_IDENTITY() as int);";

            var operatorId = await GetOperatorId(driver.OperatorName);
            
            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new
                {
                    driver.FullName,
                    driver.Address,
                    driver.ContactNumber,
                    driver.LicenseNumber,
                    driver.ExpirationDate,
                    driver.DLCodes,
                    OperatorId = operatorId
                });
            }
        }


        public async Task<IEnumerable<Driver>> GetAll()
        {
            string sql = "SELECT d.*, o.FullName AS OperatorName, v.PlateNumber AS VehicleAssigned " +
             "FROM Driver d " +
             "INNER JOIN Operator o ON d.OperatorId = o.Id " +
             "LEFT JOIN Vehicle v ON d.VehicleId = v.Id";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<Driver>(sql);
            }
        }

        public async Task<IEnumerable<Driver>> GetAllByOperatorId(int operatorId)
        {
            string sql = "SELECT d.*, o.FullName AS OperatorName, v.PlateNumber AS VehicleAssigned " +
             "FROM Driver d " +
             "INNER JOIN Operator o ON d.OperatorId = o.Id " +
             "LEFT JOIN Vehicle v ON d.VehicleId = v.Id " +
             "WHERE d.OperatorId = @OperatorId";
            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<Driver>(sql, new { OperatorId = operatorId });
            }
        }

        public async Task<Driver> GetById(int id)
        {
            var sql = "SELECT d.*, o.FullName AS OperatorName, v.PlateNumber AS VehicleAssigned " +
                      "FROM Driver d " +
                      "INNER JOIN Operator o ON d.OperatorId = o.Id " +
                      "LEFT JOIN Vehicle v ON d.VehicleId = v.Id " +
                      "WHERE d.Id = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Driver>(sql, new { Id = id });
            }
        }

        public async Task<Driver> GetByFullName(string? fullName)
        {
            var sql = "SELECT d.*, o.FullName AS OperatorName, v.PlateNumber AS VehicleAssigned " +
                      "FROM Driver d " +
                      "INNER JOIN Operator o ON d.OperatorId = o.Id " +
                      "LEFT JOIN Vehicle v ON d.VehicleId = v.Id " +
                      "WHERE d.FullName = @FullName";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Driver>(sql, new { FullName = fullName });
            }
        }

        public async Task<Driver> UpdateDriver(int id, Driver driver)
        {
            var sql = "UPDATE Driver SET [FullName] = @FullName, [ContactNumber] = @ContactNumber, " +
                      "[LicenseNumber] = @LicenseNumber, [Address] = @Address, [ExpirationDate] = @ExpirationDate, [DLCodes] = @DLCodes " +
                      "WHERE [Id] = @Id;";

            using (var con = _context.CreateConnection())
            {
                await con.ExecuteScalarAsync<int>(sql, new
                {
                    Id = id,
                    driver.FullName,
                    driver.ContactNumber,
                    driver.LicenseNumber,
                    driver.Address,
                    driver.ExpirationDate,
                    driver.DLCodes
                });
                {
                    return await GetById(id);
                }
            }
        }
        
        public async Task<Driver> UpdateVehicleAssigned(int id, string? plateNumberAssigned)
        {
            if(plateNumberAssigned.IsNullOrEmpty())
            {
                var sql = "UPDATE Driver SET [VehicleId] = NULL " +
                          "WHERE [Id] = @Id;";
                using (var con = _context.CreateConnection())
                {
                    await con.ExecuteScalarAsync<int>(sql, new
                    {
                        Id = id
                    });
                    {
                        return await GetById(id);
                    }
                }
            }
            else
            {
                var sql = "UPDATE Driver SET [VehicleId] = @VehicleId " +
                          "WHERE [Id] = @Id;";
                var vehicleId = await GetVehicleIdByPlateNumber(plateNumberAssigned);
                using (var con = _context.CreateConnection())
                {
                    await con.ExecuteScalarAsync<int>(sql, new
                    {
                        Id = id,
                        VehicleId = vehicleId
                    });
                    {
                        return await GetById(id);
                    }
                } 
            }
        }

        public async Task<bool> DeleteDriver(int id)
        {
            var sql = "DELETE FROM Driver WHERE [Id] = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteAsync(sql, new { Id = id }) > 0;
            }
        }

        private async Task<int> GetOperatorId(string? operatorName)
        {
            {
                var sql = "SELECT DISTINCT Id From Operator WHERE FullName = @OperatorName";

                using (var con = _context.CreateConnection())
                {
                    return await con.ExecuteScalarAsync<int>(sql, new { OperatorName = operatorName });
                }
            }
        }

        private async Task<int> GetVehicleIdByPlateNumber(string? plateNumber)
        {
            var sql = "SELECT [Id] FROM Vehicle WHERE [PlateNumber] = @PlateNumber";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<int>(sql, new { PlateNumber = plateNumber });
            }
        }
    }
}
