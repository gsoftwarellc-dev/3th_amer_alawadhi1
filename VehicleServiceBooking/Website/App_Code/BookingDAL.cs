using System;
using System.Data;
using System.Data.SqlClient;

namespace VehicleServiceBooking.App_Code
{
    public class BookingDAL
    {
        public int Insert(ServiceBooking booking)
        {
            object result = DbHelper.ExecuteScalarInsert("sp_Booking_Insert",
                new SqlParameter("@VehicleID", booking.VehicleID),
                new SqlParameter("@ServiceType", booking.ServiceType),
                new SqlParameter("@BookingDate", booking.BookingDate),
                new SqlParameter("@Status", booking.Status),
                new SqlParameter("@Notes", (object)booking.Notes ?? DBNull.Value));

            return result != null ? Convert.ToInt32(result) : 0;
        }

        public DataTable GetAll()
        {
            return DbHelper.ExecuteQuery("sp_Booking_GetAll");
        }

        public DataTable GetByVehicle(int vehicleId)
        {
            return DbHelper.ExecuteQuery("sp_Booking_GetByVehicle",
                new SqlParameter("@VehicleID", vehicleId));
        }

        public DataTable GetById(int bookingId)
        {
            return DbHelper.ExecuteQuery("sp_Booking_GetById",
                new SqlParameter("@BookingID", bookingId));
        }

        public int UpdateStatus(int bookingId, string status)
        {
            return DbHelper.ExecuteNonQuery("sp_Booking_UpdateStatus",
                new SqlParameter("@BookingID", bookingId),
                new SqlParameter("@Status", status));
        }

        public int Update(ServiceBooking booking)
        {
            return DbHelper.ExecuteNonQuery("sp_Booking_Update",
                new SqlParameter("@BookingID", booking.BookingID),
                new SqlParameter("@ServiceType", booking.ServiceType),
                new SqlParameter("@BookingDate", booking.BookingDate),
                new SqlParameter("@Status", booking.Status),
                new SqlParameter("@Notes", (object)booking.Notes ?? DBNull.Value));
        }

        public int Delete(int bookingId)
        {
            return DbHelper.ExecuteNonQuery("sp_Booking_Delete",
                new SqlParameter("@BookingID", bookingId));
        }

        public DataTable SearchFilter(string status, DateTime? bookingDate, string plateNumber)
        {
            return DbHelper.ExecuteQuery("sp_Booking_SearchFilter",
                new SqlParameter("@Status", (object)status ?? DBNull.Value),
                new SqlParameter("@BookingDate", (object)bookingDate ?? DBNull.Value),
                new SqlParameter("@PlateNumber", (object)plateNumber ?? DBNull.Value));
        }

        public DataTable ReportBookingsByDate()
        {
            return DbHelper.ExecuteQuery("sp_Report_BookingsByDate");
        }

        public DataTable ReportBookingsByStatus(string status)
        {
            return DbHelper.ExecuteQuery("sp_Report_BookingsByStatus",
                new SqlParameter("@Status", (object)status ?? DBNull.Value));
        }

        public DataTable ReportVehicleServiceHistory(int vehicleId)
        {
            return DbHelper.ExecuteQuery("sp_Report_VehicleServiceHistory",
                new SqlParameter("@VehicleID", vehicleId));
        }

        public DataTable ReportCustomerVehicleList()
        {
            return DbHelper.ExecuteQuery("sp_Report_CustomerVehicleList");
        }
    }
}
