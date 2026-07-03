using System;

namespace VehicleServiceBooking.App_Code
{
    public class ServiceBooking
    {
        public int BookingID { get; set; }
        public int VehicleID { get; set; }
        public string ServiceType { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
