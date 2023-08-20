using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;
using TravelProject1._0.ViewModel;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly TravelProjectContext _db;

        public AdminController(TravelProjectContext db)
        {
            _db = db;
        }

        public async  Task<IActionResult> Admin()
        {
            var result = _db.Users.Select(x => new AdminViewModel
            {
                Address = x.Address,
                Gender = x.Gender,
                Name = x.Name,
                UserId = x.UserId
            }).ToList();
            return View(result);
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
