using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserManage()
        {
            return View();
        }

        public IActionResult OrdersManage()
        {
            return View();
        }
    }
}
