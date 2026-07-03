using Microsoft.AspNetCore.Mvc;
using VehicleServiceBooking.Data;
using VehicleServiceBooking.Models;

namespace VehicleServiceBooking.Controllers
{
    public class ServiceHistoryController : Controller
    {
        private readonly BookingRepository _bookingRepository;
        private readonly VehicleRepository _vehicleRepository;

        public ServiceHistoryController(BookingRepository bookingRepository, VehicleRepository vehicleRepository)
        {
            _bookingRepository = bookingRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IActionResult> Index(string? status, DateTime? bookingDate, string? plateNumber, int? vehicleId)
        {
            var viewModel = new ServiceHistoryViewModel
            {
                Search = new BookingSearchViewModel
                {
                    Status = status,
                    BookingDate = bookingDate,
                    PlateNumber = plateNumber
                },
                Vehicles = await _vehicleRepository.GetAllAsync(),
                SelectedVehicleId = vehicleId
            };

            bool hasFilter = !string.IsNullOrWhiteSpace(status) || bookingDate.HasValue || !string.IsNullOrWhiteSpace(plateNumber);
            viewModel.Search.Results = hasFilter
                ? await _bookingRepository.SearchFilterAsync(status, bookingDate, plateNumber)
                : await _bookingRepository.GetAllAsync();

            viewModel.ByDate = await _bookingRepository.ReportBookingsByDateAsync();
            viewModel.ByStatus = await _bookingRepository.ReportBookingsByStatusAsync(null);
            viewModel.CustomerVehicles = await _bookingRepository.ReportCustomerVehicleListAsync();

            if (vehicleId.HasValue && vehicleId.Value > 0)
            {
                viewModel.VehicleHistory = await _bookingRepository.ReportVehicleServiceHistoryAsync(vehicleId.Value);
            }

            return View(viewModel);
        }
    }
}
