using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers
{
    public class TransportController : Controller
    {
        private readonly TravelProjectAzureContext _dbContext;
        public TransportController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Plan()
        {
            return View();
        }
    }
}
