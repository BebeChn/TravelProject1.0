using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class MerchandiseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Merchandise()
        {
            return View();
        }

    }
}
