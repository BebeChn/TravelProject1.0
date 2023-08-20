using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly TravelUserContext _db;

        public AdminController(TravelUserContext db)
        {
            _db = db;
        }

        public IActionResult Admin()
        {
            return _db.User !-=null?
                View(_db.User);
        }

        public IActionResult Merchandise()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }


    }
}
