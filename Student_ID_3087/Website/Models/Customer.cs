using System.ComponentModel.DataAnnotations;

namespace VehicleServiceBooking.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9+\s-]{7,20}$", ErrorMessage = "Enter a valid phone number.")]
        public string Phone { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}
