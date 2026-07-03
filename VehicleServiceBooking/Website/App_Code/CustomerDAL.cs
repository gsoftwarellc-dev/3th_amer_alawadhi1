using System.Data;
using System.Data.SqlClient;

namespace VehicleServiceBooking.App_Code
{
    /// <summary>
    /// Data access for the Customers table. Every method calls a
    /// stored procedure with parameters - no inline SQL, no string
    /// concatenation of user input.
    /// </summary>
    public class CustomerDAL
    {
        public int Insert(Customer customer)
        {
            object result = DbHelper.ExecuteScalarInsert("sp_Customer_Insert",
                new SqlParameter("@FullName", customer.FullName),
                new SqlParameter("@Email", customer.Email),
                new SqlParameter("@Phone", customer.Phone));

            return result != null ? System.Convert.ToInt32(result) : 0;
        }

        public DataTable GetAll()
        {
            return DbHelper.ExecuteQuery("sp_Customer_GetAll");
        }

        public DataTable GetById(int customerId)
        {
            return DbHelper.ExecuteQuery("sp_Customer_GetById",
                new SqlParameter("@CustomerID", customerId));
        }

        public int Update(Customer customer)
        {
            return DbHelper.ExecuteNonQuery("sp_Customer_Update",
                new SqlParameter("@CustomerID", customer.CustomerID),
                new SqlParameter("@FullName", customer.FullName),
                new SqlParameter("@Email", customer.Email),
                new SqlParameter("@Phone", customer.Phone));
        }

        public int Delete(int customerId)
        {
            return DbHelper.ExecuteNonQuery("sp_Customer_Delete",
                new SqlParameter("@CustomerID", customerId));
        }
    }
}
