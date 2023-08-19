using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Merchandise()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }


    }
}
