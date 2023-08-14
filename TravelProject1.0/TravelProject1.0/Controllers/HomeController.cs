using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TravelProject1._0.Models;

namespace TravelProject1._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
<<<<<<< HEAD
        public TravelUsersContext _context;

        public HomeController(ILogger<HomeController> logger, TravelUsersContext context)
=======
        private readonly TravelUsersContext _context;
        public HomeController(ILogger<HomeController> logger)
>>>>>>> main
        {
            _logger = logger;
            _context = context;
        }
<<<<<<< HEAD
     
        public IActionResult Index()
        {
            var product = _context.Products.OrderByDescending(p => p.ProductId);
            return View(product);
=======

        public IActionResult Index(int? PId)
        {
           
            //List<Product> products = new List<Product>();


            //if (PId != null)
            //{
            //    var result = _context.Categories.Single(x => x.CategoryId.Equals(PId));
            //    /*products = _context.Entry(result).Collection(x => x.C).Query().ToList()*/;
            //}
            //else
            //{
            //    products = _context.Products.Include(p => p.CategoryId).ToList();
            //}

            return View();
>>>>>>> main
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}