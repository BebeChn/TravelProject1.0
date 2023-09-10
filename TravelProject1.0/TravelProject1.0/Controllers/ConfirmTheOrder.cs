using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers
{
    public class ConfirmTheOrder : Controller
    {
        [HttpPost]
        public IActionResult Index(string key)
        {
            ViewBag.Key = key;
            return View();
        }
    }
}
