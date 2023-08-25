using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class OrderController : Controller
    {

        private readonly TravelProjectAzureContext _db;

        public OrderController(TravelProjectAzureContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Order()
        {
            return View();
        }



        [HttpGet]
        public async Task<IEnumerable<OrderDTO>> OrderSeach()
        {
            return _db.Orders.Select(x => new OrderDTO
            {

                OrderId = x.OrderId,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
  


            });

        }






        [HttpPost]
        public async Task<IEnumerable<OrderDTO>> OrderSeachL(OrderDTO order )
        {


            return _db.Orders.Where(y =>
            y.OrderId == order.OrderId||
            y.UserId == order.UserId||
            y.OrderDate ==order.OrderDate
            ).Select(x => new OrderDTO
            {
                OrderId = x.OrderId,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
            });


  

        }













    }
}
