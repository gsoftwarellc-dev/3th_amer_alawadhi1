using Microsoft.AspNetCore.Mvc;

namespace VehicleServiceBooking.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
