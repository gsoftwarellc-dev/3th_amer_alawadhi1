using Microsoft.Data.SqlClient;
using VehicleServiceBooking.Models;

namespace VehicleServiceBooking.Data
{
    public class CustomerRepository
    {
        private readonly DbHelper _db;

        public CustomerRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<int> InsertAsync(Customer customer)
        {
            var result = await _db.ExecuteScalarInsertAsync("sp_Customer_Insert",
                new SqlParameter("@FullName", customer.FullName),
                new SqlParameter("@Email", customer.Email),
                new SqlParameter("@Phone", customer.Phone));

            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var list = new List<Customer>();
            await using var reader = await _db.ExecuteReaderAsync("sp_Customer_GetAll");
            while (await reader.ReadAsync())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        public async Task<Customer?> GetByIdAsync(int customerId)
        {
            await using var reader = await _db.ExecuteReaderAsync("sp_Customer_GetById",
                new SqlParameter("@CustomerID", customerId));
            if (await reader.ReadAsync())
            {
                return Map(reader);
            }
            return null;
        }

        public Task<int> UpdateAsync(Customer customer)
        {
            return _db.ExecuteNonQueryAsync("sp_Customer_Update",
                new SqlParameter("@CustomerID", customer.CustomerID),
                new SqlParameter("@FullName", customer.FullName),
                new SqlParameter("@Email", customer.Email),
                new SqlParameter("@Phone", customer.Phone));
        }

        public Task<int> DeleteAsync(int customerId)
        {
            return _db.ExecuteNonQueryAsync("sp_Customer_Delete",
                new SqlParameter("@CustomerID", customerId));
        }

        private static Customer Map(SqlDataReader reader) => new Customer
        {
            CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
            FullName = reader.GetString(reader.GetOrdinal("FullName")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            Phone = reader.GetString(reader.GetOrdinal("Phone")),
            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
        };
    }
}
