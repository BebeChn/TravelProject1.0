using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Controllers
{
    public class CartController : Controller
    {
        public IActionResult AddCart()
        {
            return View();
        }
    }
}
