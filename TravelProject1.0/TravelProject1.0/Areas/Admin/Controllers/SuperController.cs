using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("api/[controller]/[action]")]
	public class SuperController : Controller
	{
		public IActionResult Super()
		{
			return View();
		}
	}
}
