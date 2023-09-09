using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
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
            return _db.Orders.Select(x => new OrderDTO
            {

                OrderId = x.OrderId,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
  


            });

        }






        
        public async Task<IEnumerable<OrderDTO>> OrderSeachL(OrderDTO order )
        {
            if (order.OrderId != 0)
            {
                return _db.Orders.Where(y =>
                    y.OrderId == (order.OrderId)
                    || y.UserId == (order.UserId)
                    //|| y.OrderDate == (order.OrderDate)
                    ).Select(x => new OrderDTO
                    {
                        OrderId = x.OrderId,
                        UserId = x.UserId,
                        OrderDate = x.OrderDate,
                    });
            }

            return _db.Orders.Select(x => new OrderDTO
            {
                OrderId = x.OrderId,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
            });

            //try
            //{
            //    _db.Orders.Where(y =>
            //   y.OrderId == (order.OrderId)
            //   || y.UserId == (order.UserId)
            //   || y.OrderDate == (order.OrderDate)
            //   ).Select(x => new OrderDTO
            //   {
            //    OrderId = x.OrderId,
            //    UserId = x.UserId,
            //    OrderDate = x.OrderDate,
            //          });
            //}catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //return null;

        }

        [HttpPut("{id}")]
        //PUT
        public async Task<string> OrderPUT(int id,OrderDTO order)
        {
            if (id != order.OrderId)
            {
                return "修改失敗";
            }

            var ORD = await _db.Orders.FindAsync(id);
            ORD.OrderId=order.OrderId;            //至少要有orderId  跟要改項目
            //ORD.UserId=order.UserId;
            ORD.OrderDate=order.OrderDate;
            _db.Entry(ORD).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return "成功";
        }


        //

        [HttpDelete("{id}")]

        public async Task<string> OrderDELETE(int id)
        {

            if (_db.Orders == null)
            {
                return "刪除失敗";
            }
            var ORD = await _db.Orders.FindAsync(id);
            if (ORD == null)
            {
                return "刪除失敗";
            }
            try
            {
                _db.Orders.Remove(ORD);
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
