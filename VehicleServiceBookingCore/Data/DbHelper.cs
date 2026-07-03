using Microsoft.Data.SqlClient;

namespace VehicleServiceBooking.Data
{
    /// <summary>
    /// Central helper for opening connections and running parameterized
    /// stored procedure commands. All data access in this project goes
    /// through here so no code ever concatenates SQL strings.
    /// </summary>
    public class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("VehicleServiceDB")
                ?? throw new InvalidOperationException("Connection string 'VehicleServiceDB' not found.");
        }

        public SqlConnection GetConnection() => new SqlConnection(_connectionString);

        public async Task<SqlDataReader> ExecuteReaderAsync(string procedureName, params SqlParameter[] parameters)
        {
            var con = GetConnection();
            var cmd = new SqlCommand(procedureName, con) { CommandType = System.Data.CommandType.StoredProcedure };
            if (parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            await con.OpenAsync();
            return await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
        }

        public async Task<int> ExecuteNonQueryAsync(string procedureName, params SqlParameter[] parameters)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand(procedureName, con) { CommandType = System.Data.CommandType.StoredProcedure };
            if (parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            await con.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<object?> ExecuteScalarInsertAsync(string procedureName, params SqlParameter[] parameters)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand(procedureName, con) { CommandType = System.Data.CommandType.StoredProcedure };
            if (parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            await con.OpenAsync();
            return await cmd.ExecuteScalarAsync();
        }
    }
}
