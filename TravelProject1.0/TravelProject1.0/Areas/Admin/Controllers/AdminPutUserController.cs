using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminPutUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
