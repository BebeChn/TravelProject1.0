using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Order()
        {
            return View();
        }
    }
}
