using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Controllers
{
	public class BookController : Controller
	{
		
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Plan()
		{
			return View();
		}
	}
}
