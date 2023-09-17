using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("OrderManage/[action]")]
    public class OrderManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
