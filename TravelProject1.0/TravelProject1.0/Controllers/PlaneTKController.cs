using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers
{
    public class PlaneTKController : Controller
    {
        private readonly TravelProjectContext _db;

        public PlaneTKController(TravelProjectContext db)
        {
            _db = db;
        }


        public IActionResult PlaneTK_catgory()
        {
            return View();
        }
        public IActionResult Plane_sale()
        {
            return View();
        }


        public async Task<IEnumerable<PlaneTK_catgoryDTO>> PlaneTK_catgoryGET()
        {
            return _db.Users.Select(x => new PlaneTK_catgoryDTO
            {

                Name = x.Name,
            });
        }


    }
}
