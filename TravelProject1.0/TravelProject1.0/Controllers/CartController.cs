using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index()
        {
            //向 Session 取得商品列表
            List<CartViewModel> CartItems = Session.
                GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");

            //計算商品總額
            if (CartItems != null)
            {
                ViewBag.Total = CartItems.Sum(m => m.CartPrice);
            }
            else
            {
                ViewBag.Total = 0;
            }

            return View();
        }
    }
}