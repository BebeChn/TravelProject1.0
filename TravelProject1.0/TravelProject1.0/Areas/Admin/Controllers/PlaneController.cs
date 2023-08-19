using Microsoft.AspNetCore.Mvc;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlaneController : Controller
    {

        public IActionResult PlaneTK_catgory()
        {
            return View();
        }
        public IActionResult Plane_sale()
        { 
            return View();
        }

    }
}
