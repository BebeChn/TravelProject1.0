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
                Id=x.Id,
                OrderId = x.OrderId,
                PlanId = x.PlanId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                UseDate = x.UseDate,



            });

        }






        
        public async Task<IEnumerable<OrderDTO>> OrderSeachL(OrderDTO order )
        {
            //if (order.OrderId != 0)
            //{
            //    return _db.OrderDetails.Include(t => t.Plan)                   
            //        .Where(y =>
            //        y.OrderId == (order.OrderId)
            //        || y.PlanId == (order.PlanId)
            //        //|| y.OrderDate == (order.OrderDate)
            //        )
            //        .OrderByDescending(o => o.Id)
            //        .Select(x => new OrderDTO
            //        {
            //            Id = x.Id,
            //            OrderId = x.OrderId,                        
            //            PlanId = x.PlanId,
            //            Quantity = x.Quantity,
            //            UnitPrice = x.UnitPrice,
            //            UseDate = x.UseDate,
            //            Name= x.Plan.Name,
            //        }
            //        );
            //}

            //return _db.OrderDetails.Include(t => t.Plan).OrderByDescending(o => o.Id).Select(x => new OrderDTO
            //{
            //    Id = x.Id,
            //    OrderId = x.OrderId,
            //    PlanId = x.PlanId,
            //    Quantity = x.Quantity,
            //    UnitPrice = x.UnitPrice,
            //    UseDate = x.UseDate,
            //    Name = x.Plan.Name,
            //});



            return _db.OrderDetails
                .Include(t => t.Plan)
                .OrderByDescending(o => o.Id)
                .GroupBy(x => x.OrderId)
                .Select(group => new OrderDTO
                {
                    Id = group.Select(x => x.Id).SingleOrDefault(),
                    OrderId = group.Key,
                    PlanId = group.Select(x => x.PlanId).SingleOrDefault(), // 将多个 PlanId 添加到列表中
                    Quantity = (short?)group.Sum(x => x.Quantity),
                    UnitPrice = group.Average(x => x.UnitPrice),
                    UseDate = group.Max(x => x.UseDate),
                    Name = group.Select(x => x.Plan.Name).SingleOrDefault() // 使用 FirstOrDefault() 获取名称
                })
                .ToList();





        }

        [HttpPut("{orderId}/{planId}")]
        //PUT
        public async Task<string> OrderPUT(int orderId, int planId, OrderDTO order)
        {
            if (orderId != order.OrderId)
            {
                return "修改失敗";
            }

            var ORD = await _db.OrderDetails.Where(od=>od.OrderId== orderId && od.PlanId == planId).FirstOrDefaultAsync();

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




        [HttpDelete("{orderId}/{planId}")]
        public async Task<string> OrderDELETE(int orderId,int planId)
        {

            if (_db.OrderDetails == null)
            {
                return "刪除失敗";
            }
            var ORD = await _db.OrderDetails.Where(od => od.OrderId == orderId && od.PlanId == planId).FirstOrDefaultAsync();
            if (ORD == null)
            {
                return "刪除失敗";
            }
            try
            {
                _db.OrderDetails.Remove(ORD);
                await _db.SaveChangesAsync();
            }
            catch
            {
                return "刪除關聯失敗";
            }
            return "刪除成功";
        }







    }
}
