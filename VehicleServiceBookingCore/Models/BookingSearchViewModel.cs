namespace VehicleServiceBooking.Models
{
    public class BookingSearchViewModel
    {
        public string? Status { get; set; }
        public DateTime? BookingDate { get; set; }
        public string? PlateNumber { get; set; }
        public List<ServiceBooking> Results { get; set; } = new();
    }
}
