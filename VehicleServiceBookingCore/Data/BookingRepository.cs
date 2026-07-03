using Microsoft.Data.SqlClient;
using VehicleServiceBooking.Models;

namespace VehicleServiceBooking.Data
{
    public class BookingRepository
    {
        private readonly DbHelper _db;

        public BookingRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<int> InsertAsync(ServiceBooking booking)
        {
            var result = await _db.ExecuteScalarInsertAsync("sp_Booking_Insert",
                new SqlParameter("@VehicleID", booking.VehicleID),
                new SqlParameter("@ServiceType", booking.ServiceType),
                new SqlParameter("@BookingDate", booking.BookingDate),
                new SqlParameter("@Status", booking.Status),
                new SqlParameter("@Notes", (object?)booking.Notes ?? DBNull.Value));

            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<List<ServiceBooking>> GetAllAsync()
        {
            var list = new List<ServiceBooking>();
            await using var reader = await _db.ExecuteReaderAsync("sp_Booking_GetAll");
            while (await reader.ReadAsync())
            {
                list.Add(MapWithJoins(reader));
            }
            return list;
        }

        public async Task<ServiceBooking?> GetByIdAsync(int bookingId)
        {
            await using var reader = await _db.ExecuteReaderAsync("sp_Booking_GetById",
                new SqlParameter("@BookingID", bookingId));
            if (await reader.ReadAsync())
            {
                return Map(reader);
            }
            return null;
        }

        public Task<int> UpdateAsync(ServiceBooking booking)
        {
            return _db.ExecuteNonQueryAsync("sp_Booking_Update",
                new SqlParameter("@BookingID", booking.BookingID),
                new SqlParameter("@ServiceType", booking.ServiceType),
                new SqlParameter("@BookingDate", booking.BookingDate),
                new SqlParameter("@Status", booking.Status),
                new SqlParameter("@Notes", (object?)booking.Notes ?? DBNull.Value));
        }

        public Task<int> DeleteAsync(int bookingId)
        {
            return _db.ExecuteNonQueryAsync("sp_Booking_Delete",
                new SqlParameter("@BookingID", bookingId));
        }

        public async Task<List<ServiceBooking>> SearchFilterAsync(string? status, DateTime? bookingDate, string? plateNumber)
        {
            var list = new List<ServiceBooking>();
            await using var reader = await _db.ExecuteReaderAsync("sp_Booking_SearchFilter",
                new SqlParameter("@Status", (object?)status ?? DBNull.Value),
                new SqlParameter("@BookingDate", (object?)bookingDate ?? DBNull.Value),
                new SqlParameter("@PlateNumber", (object?)plateNumber ?? DBNull.Value));
            while (await reader.ReadAsync())
            {
                list.Add(MapWithJoins(reader));
            }
            return list;
        }

        public async Task<List<ServiceBooking>> ReportBookingsByDateAsync()
        {
            var list = new List<ServiceBooking>();
            await using var reader = await _db.ExecuteReaderAsync("sp_Report_BookingsByDate");
            while (await reader.ReadAsync())
            {
                list.Add(MapReportRow(reader));
            }
            return list;
        }

        public async Task<List<ServiceBooking>> ReportBookingsByStatusAsync(string? status)
        {
            var list = new List<ServiceBooking>();
            await using var reader = await _db.ExecuteReaderAsync("sp_Report_BookingsByStatus",
                new SqlParameter("@Status", (object?)status ?? DBNull.Value));
            while (await reader.ReadAsync())
            {
                list.Add(MapReportRow(reader));
            }
            return list;
        }

        public async Task<List<ServiceBooking>> ReportVehicleServiceHistoryAsync(int vehicleId)
        {
            var list = new List<ServiceBooking>();
            await using var reader = await _db.ExecuteReaderAsync("sp_Report_VehicleServiceHistory",
                new SqlParameter("@VehicleID", vehicleId));
            while (await reader.ReadAsync())
            {
                list.Add(new ServiceBooking
                {
                    BookingID = reader.GetInt32(reader.GetOrdinal("BookingID")),
                    ServiceType = reader.GetString(reader.GetOrdinal("ServiceType")),
                    BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
                });
            }
            return list;
        }

        public async Task<List<CustomerVehicleRow>> ReportCustomerVehicleListAsync()
        {
            var list = new List<CustomerVehicleRow>();
            await using var reader = await _db.ExecuteReaderAsync("sp_Report_CustomerVehicleList");
            while (await reader.ReadAsync())
            {
                list.Add(new CustomerVehicleRow
                {
                    CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                    VehicleID = reader.IsDBNull(reader.GetOrdinal("VehicleID")) ? null : reader.GetInt32(reader.GetOrdinal("VehicleID")),
                    PlateNumber = reader.IsDBNull(reader.GetOrdinal("PlateNumber")) ? null : reader.GetString(reader.GetOrdinal("PlateNumber")),
                    Brand = reader.IsDBNull(reader.GetOrdinal("Brand")) ? null : reader.GetString(reader.GetOrdinal("Brand")),
                    Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader.GetString(reader.GetOrdinal("Model")),
                    Year = reader.IsDBNull(reader.GetOrdinal("Year")) ? null : reader.GetInt32(reader.GetOrdinal("Year"))
                });
            }
            return list;
        }

        private static ServiceBooking Map(SqlDataReader reader) => new ServiceBooking
        {
            BookingID = reader.GetInt32(reader.GetOrdinal("BookingID")),
            VehicleID = reader.GetInt32(reader.GetOrdinal("VehicleID")),
            ServiceType = reader.GetString(reader.GetOrdinal("ServiceType")),
            BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
            Status = reader.GetString(reader.GetOrdinal("Status")),
            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
        };

        private static ServiceBooking MapWithJoins(SqlDataReader reader) => new ServiceBooking
        {
            BookingID = reader.GetInt32(reader.GetOrdinal("BookingID")),
            VehicleID = reader.GetInt32(reader.GetOrdinal("VehicleID")),
            OwnerName = reader.GetString(reader.GetOrdinal("OwnerName")),
            PlateNumber = reader.GetString(reader.GetOrdinal("PlateNumber")),
            Brand = reader.GetString(reader.GetOrdinal("Brand")),
            Model = reader.GetString(reader.GetOrdinal("Model")),
            ServiceType = reader.GetString(reader.GetOrdinal("ServiceType")),
            BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
            Status = reader.GetString(reader.GetOrdinal("Status")),
            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
        };

        private static ServiceBooking MapReportRow(SqlDataReader reader) => new ServiceBooking
        {
            BookingID = reader.GetInt32(reader.GetOrdinal("BookingID")),
            PlateNumber = reader.GetString(reader.GetOrdinal("PlateNumber")),
            OwnerName = reader.GetString(reader.GetOrdinal("OwnerName")),
            ServiceType = reader.GetString(reader.GetOrdinal("ServiceType")),
            BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
            Status = reader.GetString(reader.GetOrdinal("Status"))
        };
    }
}
