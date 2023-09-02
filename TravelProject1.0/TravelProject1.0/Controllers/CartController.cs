using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelProject1._0.Helper;
using TravelProject1._0.Models;
using TravelProject1._0.Models.ViewModel;

namespace TravelProject1._0.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;
        public CartController(ILogger<HomeController> logger, TravelProjectAzureContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            var userClaims = HttpContext.User.Claims;
            foreach (var claim in userClaims)
            {
                if (claim.Type == ClaimTypes.NameIdentifier)
                {
                    string userId = claim.Value;
                    Console.WriteLine("User ID: " + userId);
                }  
            }

            //向 Session 取得商品列表
            List<CartViewModel> CartItems = Session.
                GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");

            return View();
        }
       
       
    }
}