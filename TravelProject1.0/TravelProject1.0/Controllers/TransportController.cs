using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;

namespace TravelProject1._0.Controllers
{
    public class TransportController : Controller
    {
        private readonly TravelProjectAzureContext _dbContext;
        public TransportController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Transport/Index
        public IActionResult Index()
        {
            return View();
        }

        //Transport/Plan/{id}
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Plan(int? id)
        {
            if (id == null || _dbContext.Products == null) return NotFound();

            var plan = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (plan == null) return NotFound();

            return View(plan);
        }
    }
}
