using Microsoft.AspNetCore.Mvc;
using VehicleServiceBooking.Data;
using VehicleServiceBooking.Models;

namespace VehicleServiceBooking.Controllers
{
    public class BookingsController : Controller
    {
        private readonly BookingRepository _bookingRepository;
        private readonly VehicleRepository _vehicleRepository;

        public BookingsController(BookingRepository bookingRepository, VehicleRepository vehicleRepository)
        {
            _bookingRepository = bookingRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Vehicles = await _vehicleRepository.GetAllAsync();
            var bookings = await _bookingRepository.GetAllAsync();
            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceBooking booking)
        {
            if (booking.VehicleID <= 0
                || string.IsNullOrWhiteSpace(booking.ServiceType)
                || Array.IndexOf(ServiceBooking.ServiceTypes, booking.ServiceType) < 0
                || booking.BookingDate == default)
            {
                TempData["StatusMessage"] = "Please select a vehicle, a valid service type, and a booking date.";
                TempData["StatusIsError"] = true;
                return RedirectToAction(nameof(Index));
            }

            booking.Status = "Pending";
            await _bookingRepository.InsertAsync(booking);
            TempData["StatusMessage"] = $"Service booking created for {booking.ServiceType} on {booking.BookingDate:yyyy-MM-dd}.";
            TempData["StatusIsError"] = false;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int bookingId, DateTime bookingDate, string status, string? notes)
        {
            if (bookingDate == default || Array.IndexOf(ServiceBooking.Statuses, status) < 0)
            {
                TempData["StatusMessage"] = "Please provide a valid date and status when editing.";
                TempData["StatusIsError"] = true;
                return RedirectToAction(nameof(Index));
            }

            var existing = await _bookingRepository.GetByIdAsync(bookingId);
            if (existing == null)
            {
                TempData["StatusMessage"] = "Booking not found.";
                TempData["StatusIsError"] = true;
                return RedirectToAction(nameof(Index));
            }

            existing.BookingDate = bookingDate;
            existing.Status = status;
            existing.Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();

            await _bookingRepository.UpdateAsync(existing);
            TempData["StatusMessage"] = "Booking updated successfully.";
            TempData["StatusIsError"] = false;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int bookingId)
        {
            await _bookingRepository.DeleteAsync(bookingId);
            TempData["StatusMessage"] = "Booking cancelled.";
            TempData["StatusIsError"] = false;
            return RedirectToAction(nameof(Index));
        }
    }
}
