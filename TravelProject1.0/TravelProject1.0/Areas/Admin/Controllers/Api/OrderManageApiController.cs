using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Areas.Admin.Models.DTO;
using TravelProject1._0.Models;

namespace TravelProject1._0.Areas.Admin.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderManageApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;
        public OrderManageApiController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        //取得訂單
        [HttpGet]
        public async Task<object> GetOrders()
        {
            var result = await _dbContext.OrderDetails.AsNoTracking().Include(t => t.Order).GroupBy(x => x.OrderId).Select(x => new
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
                    UseDate = z.UseDate.HasValue ? z.UseDate.Value.ToString("yyyy-MM-dd") : ""
                })
            }).ToListAsync();

            return result;
        }

        //修改訂單
        [HttpPut("{orderId}")]
        public async Task<bool> UpdateOrder(int orderId, OrderUpdateDTO data)
        {
            try
            {
                var order = _dbContext.Orders.FirstOrDefault(x => x.OrderId == orderId);
                if (order == null) return false;
                order.OrderDate = data.OrderDate;
                order.Status = data.Status;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //刪除訂單
        [HttpDelete("{orderId}")]
        public async Task<bool> DeleteOrder(int orderId)
        {
            try
            {
                var od = _dbContext.OrderDetails.Where(x => x.OrderId == orderId);
                _dbContext.OrderDetails.RemoveRange(od);
                var order = _dbContext.Orders.First(x => x.OrderId == orderId);
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //修改訂單明細
        [HttpPut("{Id}")]
        public async Task<bool> UpdateDetail(int Id, DetailUpdateDTO data)
        {
            try
            {
                var od = _dbContext.OrderDetails.FirstOrDefault(x => x.Id == Id);
                if (od == null) return false;
                od.UseDate = data.UseDate;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //刪除訂單明細
        [HttpDelete("{id}")]
        public async Task<bool> DeleteDetail(int id)
        {
            try
            {
                var od = _dbContext.OrderDetails.FirstOrDefault(x => x.Id == id);
                if (od == null) return false;
                _dbContext.OrderDetails.Remove(od);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
