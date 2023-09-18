using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Route("AdminPutUser/[action]")]
	public class AdminPutUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ManageProducts()
        {
            return View();
        }
    }
}
