using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models.ViewModel;

namespace TravelProject1._0.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ChartController : Controller
	{
		public IActionResult UserAnalyze()
		{
			return View();
		}

		public IActionResult SalesAnalyze()
		{
			return View();
		}

		public IActionResult CurrentMonthSale()
		{
			return View();
		}

	}
}
