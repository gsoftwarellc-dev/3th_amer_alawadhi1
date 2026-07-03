using System.Data;
using System.Data.SqlClient;

namespace VehicleServiceBooking.App_Code
{
    public class VehicleDAL
    {
        public int Insert(Vehicle vehicle)
        {
            object result = DbHelper.ExecuteScalarInsert("sp_Vehicle_Insert",
                new SqlParameter("@CustomerID", vehicle.CustomerID),
                new SqlParameter("@PlateNumber", vehicle.PlateNumber),
                new SqlParameter("@Brand", vehicle.Brand),
                new SqlParameter("@Model", vehicle.Model),
                new SqlParameter("@Year", vehicle.Year));

            return result != null ? System.Convert.ToInt32(result) : 0;
        }

        public DataTable GetAll()
        {
            return DbHelper.ExecuteQuery("sp_Vehicle_GetAll");
        }

        public DataTable GetByCustomer(int customerId)
        {
            return DbHelper.ExecuteQuery("sp_Vehicle_GetByCustomer",
                new SqlParameter("@CustomerID", customerId));
        }

        public DataTable GetById(int vehicleId)
        {
            return DbHelper.ExecuteQuery("sp_Vehicle_GetById",
                new SqlParameter("@VehicleID", vehicleId));
        }

        public int Update(Vehicle vehicle)
        {
            return DbHelper.ExecuteNonQuery("sp_Vehicle_Update",
                new SqlParameter("@VehicleID", vehicle.VehicleID),
                new SqlParameter("@PlateNumber", vehicle.PlateNumber),
                new SqlParameter("@Brand", vehicle.Brand),
                new SqlParameter("@Model", vehicle.Model),
                new SqlParameter("@Year", vehicle.Year));
        }

        public int Delete(int vehicleId)
        {
            return DbHelper.ExecuteNonQuery("sp_Vehicle_Delete",
                new SqlParameter("@VehicleID", vehicleId));
        }
    }
}
