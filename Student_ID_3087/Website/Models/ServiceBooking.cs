using System.ComponentModel.DataAnnotations;

namespace VehicleServiceBooking.Models
{
    public class ServiceBooking
    {
        public int BookingID { get; set; }

        [Required(ErrorMessage = "Please select a vehicle.")]
        public int VehicleID { get; set; }

        public string? OwnerName { get; set; }
        public string? PlateNumber { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }

        [Required(ErrorMessage = "Please select a service type.")]
        public string ServiceType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please choose a booking date.")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        public string Status { get; set; } = "Pending";

        [StringLength(500)]
        public string? Notes { get; set; }

        public static readonly string[] ServiceTypes =
        {
            "Oil Change", "Tire Rotation", "Brake Service",
            "Battery Replacement", "Full Inspection", "Air Conditioning Service"
        };

        public static readonly string[] Statuses =
        {
            "Pending", "Confirmed", "In Progress", "Completed", "Cancelled"
        };
    }
}
