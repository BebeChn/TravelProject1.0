using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/Manage/[action]")]
    public class OrderManageController : Controller
    {
        public IActionResult Orders()
        {
            return View();
        }
    }
}
