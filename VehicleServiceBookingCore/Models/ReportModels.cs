namespace VehicleServiceBooking.Models
{
    public class CustomerVehicleRow
    {
        public int CustomerID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int? VehicleID { get; set; }
        public string? PlateNumber { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
    }

    public class ServiceHistoryViewModel
    {
        public BookingSearchViewModel Search { get; set; } = new();
        public List<ServiceBooking> ByDate { get; set; } = new();
        public List<ServiceBooking> ByStatus { get; set; } = new();
        public List<Vehicle> Vehicles { get; set; } = new();
        public int? SelectedVehicleId { get; set; }
        public List<ServiceBooking> VehicleHistory { get; set; } = new();
        public List<CustomerVehicleRow> CustomerVehicles { get; set; } = new();
    }
}
