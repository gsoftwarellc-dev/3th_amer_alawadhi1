using System.ComponentModel.DataAnnotations;

namespace VehicleServiceBooking.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }

        [Required(ErrorMessage = "Please select a customer.")]
        public int CustomerID { get; set; }

        public string? OwnerName { get; set; }

        [Required(ErrorMessage = "Plate number is required.")]
        [RegularExpression(@"^[A-Za-z]{1,3}[\s-]?\d{1,6}$", ErrorMessage = "Enter a valid plate number (e.g. A12345).")]
        public string PlateNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand is required.")]
        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required.")]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year is required.")]
        [Range(1980, 2100, ErrorMessage = "Enter a reasonable vehicle year.")]
        public int Year { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
