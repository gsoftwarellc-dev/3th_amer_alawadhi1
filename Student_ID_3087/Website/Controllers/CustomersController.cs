using Microsoft.AspNetCore.Mvc;
using VehicleServiceBooking.Data;
using VehicleServiceBooking.Models;

namespace VehicleServiceBooking.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerRepository _customerRepository;

        public CustomersController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerRepository.GetAllAsync();
            return View(customers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            // Server-side validation (authoritative), mirrors client-side JS checks.
            if (!ModelState.IsValid)
            {
                TempData["StatusMessage"] = "Please correct the highlighted fields.";
                TempData["StatusIsError"] = true;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _customerRepository.InsertAsync(customer);
                TempData["StatusMessage"] = $"Customer \"{customer.FullName}\" registered successfully.";
                TempData["StatusIsError"] = false;
            }
            catch (Exception)
            {
                TempData["StatusMessage"] = "A customer with that email may already exist, or a database error occurred.";
                TempData["StatusIsError"] = true;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FullName)
                || !System.Text.RegularExpressions.Regex.IsMatch(customer.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                || !System.Text.RegularExpressions.Regex.IsMatch(customer.Phone, @"^[0-9+\s-]{7,20}$"))
            {
                TempData["StatusMessage"] = "Please provide a valid name, email, and phone number when editing.";
                TempData["StatusIsError"] = true;
                return RedirectToAction(nameof(Index));
            }

            await _customerRepository.UpdateAsync(customer);
            TempData["StatusMessage"] = "Customer updated successfully.";
            TempData["StatusIsError"] = false;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int customerId)
        {
            await _customerRepository.DeleteAsync(customerId);
            TempData["StatusMessage"] = "Customer deleted.";
            TempData["StatusIsError"] = false;
            return RedirectToAction(nameof(Index));
        }
    }
}
