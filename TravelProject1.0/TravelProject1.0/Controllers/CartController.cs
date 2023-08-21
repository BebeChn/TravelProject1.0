using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Helper;
using TravelProject1._0.Models;

namespace TravelProject1._0.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
            
    }
}
