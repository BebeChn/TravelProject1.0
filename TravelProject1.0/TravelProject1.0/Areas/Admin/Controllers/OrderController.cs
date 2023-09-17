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
                    OrderDate = x.First().Order.OrderDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                    UserId = x.First().Order.UserId,

                    od = x.Select(z => new
                    {
                        z.OrderId,
                        z.Odname,
                        z.PlanId,
                        z.Id,
                        z.Odimg,
                        z.Quantity,
                        z.UnitPrice,
                        UseDate = z.UseDate.HasValue ? z.UseDate.Value.ToString("yyyy-MM-dd"): "",
                    })
                })
                .ToListAsync();

            return result;



        }
        [HttpPut("{Id}")]
        public async Task<bool> DetailPUT(int Id, DetailUpdateDTO data)
        {
            try
            {
                var od = _db.OrderDetails.FirstOrDefault(x => x.Id == Id);
                if (od == null) return false;
                od.UseDate = data.UseDate;
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        
        [HttpPut("{orderId}")]
        //PUT
        public async Task<bool> OrderPUT(int orderId, OrderUpdateDTO data)
        {
            try
            {
                var order = _db.Orders.FirstOrDefault(x => x.OrderId == orderId);
                if (order == null) return false;

                order.OrderDate = data.OrderDate;
                order.Status = data.Status;
                await _db.SaveChangesAsync();
                return true;
                  
            }
            catch (Exception)
            {
                return false;                
            }
        }


        
[HttpDelete("{id}")]
        public async Task<bool> OrderDetailDELETE(int id)
        {
            try
            {
                var od = _db.OrderDetails.FirstOrDefault(x => x.Id == id);
                if (od == null) return false;
                _db.OrderDetails.Remove(od);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

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
