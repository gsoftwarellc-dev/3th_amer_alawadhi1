using Microsoft.Data.SqlClient;
using VehicleServiceBooking.Models;

namespace VehicleServiceBooking.Data
{
    public class VehicleRepository
    {
        private readonly DbHelper _db;

        public VehicleRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<int> InsertAsync(Vehicle vehicle)
        {
            var result = await _db.ExecuteScalarInsertAsync("sp_Vehicle_Insert",
                new SqlParameter("@CustomerID", vehicle.CustomerID),
                new SqlParameter("@PlateNumber", vehicle.PlateNumber),
                new SqlParameter("@Brand", vehicle.Brand),
                new SqlParameter("@Model", vehicle.Model),
                new SqlParameter("@Year", vehicle.Year));

            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            var list = new List<Vehicle>();
            await using var reader = await _db.ExecuteReaderAsync("sp_Vehicle_GetAll");
            while (await reader.ReadAsync())
            {
                list.Add(MapWithOwner(reader));
            }
            return list;
        }

        public async Task<List<Vehicle>> GetByCustomerAsync(int customerId)
        {
            var list = new List<Vehicle>();
            await using var reader = await _db.ExecuteReaderAsync("sp_Vehicle_GetByCustomer",
                new SqlParameter("@CustomerID", customerId));
            while (await reader.ReadAsync())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        public async Task<Vehicle?> GetByIdAsync(int vehicleId)
        {
            await using var reader = await _db.ExecuteReaderAsync("sp_Vehicle_GetById",
                new SqlParameter("@VehicleID", vehicleId));
            if (await reader.ReadAsync())
            {
                return Map(reader);
            }
            return null;
        }

        public Task<int> UpdateAsync(Vehicle vehicle)
        {
            return _db.ExecuteNonQueryAsync("sp_Vehicle_Update",
                new SqlParameter("@VehicleID", vehicle.VehicleID),
                new SqlParameter("@PlateNumber", vehicle.PlateNumber),
                new SqlParameter("@Brand", vehicle.Brand),
                new SqlParameter("@Model", vehicle.Model),
                new SqlParameter("@Year", vehicle.Year));
        }

        public Task<int> DeleteAsync(int vehicleId)
        {
            return _db.ExecuteNonQueryAsync("sp_Vehicle_Delete",
                new SqlParameter("@VehicleID", vehicleId));
        }

        private static Vehicle Map(SqlDataReader reader) => new Vehicle
        {
            VehicleID = reader.GetInt32(reader.GetOrdinal("VehicleID")),
            CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
            PlateNumber = reader.GetString(reader.GetOrdinal("PlateNumber")),
            Brand = reader.GetString(reader.GetOrdinal("Brand")),
            Model = reader.GetString(reader.GetOrdinal("Model")),
            Year = reader.GetInt32(reader.GetOrdinal("Year")),
            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
        };

        private static Vehicle MapWithOwner(SqlDataReader reader) => new Vehicle
        {
            VehicleID = reader.GetInt32(reader.GetOrdinal("VehicleID")),
            CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
            OwnerName = reader.GetString(reader.GetOrdinal("OwnerName")),
            PlateNumber = reader.GetString(reader.GetOrdinal("PlateNumber")),
            Brand = reader.GetString(reader.GetOrdinal("Brand")),
            Model = reader.GetString(reader.GetOrdinal("Model")),
            Year = reader.GetInt32(reader.GetOrdinal("Year")),
            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
        };
    }
}
