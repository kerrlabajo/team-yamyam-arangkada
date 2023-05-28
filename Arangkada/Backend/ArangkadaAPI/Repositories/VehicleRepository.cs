using ArangkadaAPI.Context;
using ArangkadaAPI.Models;
using Dapper;

namespace ArangkadaAPI.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly DapperContext _context;
        public VehicleRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddVehicle(Vehicle vehicle)
        {
            var sql = "INSERT Vehicle (CRNumber,PlateNumber, BodyType, Make, RentFee, RentStatus, OperatorId)" +
                "VALUES (@CRNumber, @PlateNumber, @BodyType, @Make, @RentFee, @RentStatus, @OperatorId);" +
                "SELECT CAST(SCOPE_IDENTITY() as int);";

            var operatorId = await GetOperatorId(vehicle.OperatorName);

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>
                    (sql, new
                    {
                        vehicle.CRNumber,
                        vehicle.PlateNumber,
                        vehicle.BodyType,
                        vehicle.Make,
                        vehicle.RentFee,
                        vehicle.RentStatus,
                        OperatorId = operatorId,
                    });
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            var sql = "SELECT v.Id, v.CRNumber, v.PlateNumber, v.BodyType, v.Make, v.RentFee, v.RentStatus, o.FullName AS OperatorName " +
                      "FROM Vehicle v " +
                      "INNER JOIN Operator o ON v.OperatorId = o.Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<Vehicle>(sql);
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAllByOperatorId(int operatorId)
        {
            var sql = "SELECT v.Id, v.CRNumber, v.PlateNumber, v.BodyType, v.Make, v.RentFee, v.RentStatus, o.FullName AS OperatorName " +
                      "FROM Vehicle v " +
                      "INNER JOIN Operator o ON v.OperatorId = o.Id " +
                      "WHERE v.OperatorId = @OperatorId;";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<Vehicle>(sql, new { OperatorId = operatorId });
            }
        }

        public async Task<Vehicle> GetById(int id)
        {
            var sql = "SELECT o.FullName AS OperatorName, v.ID, v.CRNumber, v.PlateNumber," +
                "v.BodyType, v.Make, v.RentFee, v.RentStatus " +
                "FROM Vehicle v " +
                "INNER JOIN [dbo].[Operator] o on v.OperatorId = o.ID " +
                "WHERE v.ID = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Vehicle>(sql, new { Id = id });
            }
        }

        public async Task<Vehicle> GetByPlateNumber(string? plateNumber)
        {
            var sql = "SELECT v.Id, o.FullName AS OperatorName, v.CRNumber, v.PlateNumber, v.BodyType, v.Make, v.RentFee, v.RentStatus " +
                      "FROM Vehicle v " +
                      "INNER JOIN Operator o ON v.OperatorId = o.Id " +
                      "WHERE v.PlateNumber = @PlateNumber;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Vehicle>(sql, new { PlateNumber = plateNumber });
            }
        }

        public async Task<Vehicle> UpdateRentStatus(int id, bool rentStatus)
        {
            var sql = "UPDATE Vehicle SET RentStatus = @RentStatus " +
                      "WHERE Id = @Id";

            using (var con = _context.CreateConnection())
            {
                await con.ExecuteScalarAsync<int>(sql, new { Id = id, rentStatus });
                {
                    return await GetById(id);
                }

            }
        }

        public async Task<Vehicle> UpdateRentFee(int id, double rentFee)
        {
            var sql = "UPDATE Vehicle SET RentFee = @RentFee " +
                      "WHERE Id = @Id";

            using (var con = _context.CreateConnection())
            {
                await con.ExecuteAsync(sql, new { Id = id, RentFee = rentFee });
                {
                    return await GetById(id);
                }

            }
        }

        public async Task<bool> DeleteVehicle(int id)
        {
            var sql = "DELETE FROM Vehicle WHERE [Id] = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteAsync(sql, new { Id = id }) > 0;
            }
        }

        private async Task<int> GetOperatorId(string? operatorName)
        {
            {
                var sql = "SELECT DISTINCT Id From [dbo].[Operator] WHERE FullName = @OperatorName";

                using (var con = _context.CreateConnection())
                {
                    return await con.ExecuteScalarAsync<int>(sql, new { OperatorName = operatorName });
                }
            }
        }
    }
}
