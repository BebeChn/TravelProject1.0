using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Route("[area]/Manage/[action]")]
	public class AdminPutUserController : Controller
    {
        public IActionResult User()
        {
            return View();
        }
        public IActionResult Products()
        {
            return View();
        }
    }
}
