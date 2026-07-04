using Microsoft.AspNetCore.Mvc;
using VehicleServiceBooking.Data;
using VehicleServiceBooking.Models;

namespace VehicleServiceBooking.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly VehicleRepository _vehicleRepository;
        private readonly CustomerRepository _customerRepository;

        public VehiclesController(VehicleRepository vehicleRepository, CustomerRepository customerRepository)
        {
            _vehicleRepository = vehicleRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Customers = await _customerRepository.GetAllAsync();
            var vehicles = await _vehicleRepository.GetAllAsync();
            return View(vehicles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            int currentYear = DateTime.Now.Year;

            if (vehicle.CustomerID <= 0
                || !System.Text.RegularExpressions.Regex.IsMatch(vehicle.PlateNumber ?? "", @"^[A-Za-z]{1,3}[\s-]?\d{1,6}$")
                || string.IsNullOrWhiteSpace(vehicle.Brand)
                || string.IsNullOrWhiteSpace(vehicle.Model)
                || vehicle.Year < 1980 || vehicle.Year > currentYear + 1)
            {
                TempData["StatusMessage"] = $"Please provide a customer, valid plate number, brand, model, and a reasonable year (1980 - {currentYear + 1}).";
                TempData["StatusIsError"] = true;
                return RedirectToAction(nameof(Index));
            }

            await _vehicleRepository.InsertAsync(vehicle);
            TempData["StatusMessage"] = $"Vehicle \"{vehicle.Brand} {vehicle.Model}\" registered successfully.";
            TempData["StatusIsError"] = false;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Vehicle vehicle)
        {
            int currentYear = DateTime.Now.Year;

            if (!System.Text.RegularExpressions.Regex.IsMatch(vehicle.PlateNumber ?? "", @"^[A-Za-z]{1,3}[\s-]?\d{1,6}$")
                || string.IsNullOrWhiteSpace(vehicle.Brand)
                || string.IsNullOrWhiteSpace(vehicle.Model)
                || vehicle.Year < 1980 || vehicle.Year > currentYear + 1)
            {
                TempData["StatusMessage"] = "Please provide a valid plate number, brand, model, and year when editing.";
                TempData["StatusIsError"] = true;
                return RedirectToAction(nameof(Index));
            }

            await _vehicleRepository.UpdateAsync(vehicle);
            TempData["StatusMessage"] = "Vehicle updated successfully.";
            TempData["StatusIsError"] = false;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int vehicleId)
        {
            await _vehicleRepository.DeleteAsync(vehicleId);
            TempData["StatusMessage"] = "Vehicle deleted.";
            TempData["StatusIsError"] = false;
            return RedirectToAction(nameof(Index));
        }
    }
}
