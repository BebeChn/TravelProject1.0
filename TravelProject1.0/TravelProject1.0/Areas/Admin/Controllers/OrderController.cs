using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
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

        public IActionResult Order()
        {
            return View();
        }



        [HttpGet]
        public async Task<IEnumerable<OrderDTO>> OrderGET()
        {
            return _db.OrderDetails.Select(x => new OrderDTO
            {
                Id = x.Id,
                OrderId = x.OrderId,
                PlanId = x.PlanId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                UseDate = x.UseDate,



            });

        }







        public async Task<object> OrderSeachL()
        {
            var result = await _db.OrderDetails.AsNoTracking()
                .Include(t => t.Order)
                .GroupBy(x => x.OrderId)
                .Select(x => new
                {
                    OrderId = x.Key,
                    TotalPrice = x.First().Order.TotalPrice,
                    Status = x.First().Order.Status,
                    NewPoint = x.First().Order.NewPoint,
                    OrderDate = x.First().Order.OrderDate,
                    UserId = x.First().Order.UserId,

                    od = x.Select(z => new
                    {
                        z.Odname,
                        z.PlanId,
                        z.Id,
                        z.Odimg,
                        z.Quantity,
                        z.UnitPrice,
                        z.UseDate,
                    })
                })
                .ToListAsync();

            return result;



        }

        [HttpPut("{orderId}/{planId}")]
        //PUT
        public async Task<string> OrderPUT(int orderId, int planId, OrderDTO order)
        {
            if (orderId != order.OrderId)
            {
                return "修改失敗";
            }

            var ORD = await _db.OrderDetails.Where(od => od.OrderId == orderId && od.PlanId == planId).FirstOrDefaultAsync();

            ORD.OrderId = order.OrderId;            //至少要有orderId  跟要改項目
            //ORD.PlanId=order.PlanId;
            ORD.Quantity = order.Quantity;
            ORD.UseDate = order.UseDate;

            _db.Entry(ORD).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return "成功";
        }




        [HttpDelete("{orderId}")]
        public async Task<bool> OrderDELETE(int orderId)
        {
            try
            {
                var od = _db.OrderDetails.Where(x => x.OrderId == orderId);
                _db.OrderDetails.RemoveRange(od);
                var order = _db.Orders.First(x => x.OrderId == orderId);
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }







    }
}
