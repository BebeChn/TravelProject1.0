using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Controllers
{
    public class RatingController : Controller
    {
        public IActionResult Index(string key)
        {
            ViewBag.Key = key;
            return View();
        }
    }
}
