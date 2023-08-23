using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;

namespace TravelProject1._0.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class MerchandiseController : Controller
    {
        private readonly TravelProjectAzureContext _db;

        public MerchandiseController(TravelProjectAzureContext db)
        {
            _db = db;
        }


        public IActionResult Merchandise()
        {
            return View();
        }
    }
}
