using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("AdminManage/[action]")]
	public class AdminManageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
