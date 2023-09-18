using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Drawing;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Services;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/OrderApi/[Action]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _context;
        private readonly IUserIdentityService _userIdentityService;
        public OrderApiController(IUserIdentityService userIdentityService, TravelProjectAzureContext context)
        {
            _userIdentityService = userIdentityService;
            _context = context;
        }

        [HttpGet("{id}")]
        public IEnumerable<OrderInfo> OrderDetails(int id)
        {
            var userId = _userIdentityService.GetUserId();

            return _context.OrderDetails.Include(od => od.Order).Include(od => od.Plan).Where(od => od.Order.UserId == userId && od.OrderId == id).Select(od => new OrderInfo
            {
                PlanId = od.PlanId,
                OrderId = od.OrderId,
                Odimg = od.Odimg,
                Odname = od.Odname,
                UseDate = od.UseDate,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                ProductId = od.Plan.ProductId
            });
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<UserOrderDto> UserOrder()
        {
            var userId = _userIdentityService.GetUserId();
            return _context.Orders.Where(o => o.UserId == userId).Select(o => new UserOrderDto
            {
                OrderDate = o.OrderDate.Value.ToString("yyyy-MM-dd"),
                OrderId = o.OrderId,
                Status = o.Status,
                UserId = userId
            });
        }

        [HttpGet]
        public IEnumerable<OrderGetPointDto> OrderDetailsGetPoint()
        {
            var userId = _userIdentityService.GetUserId();
            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Plan).Where(o => o.UserId == userId)
                .Select(o => new OrderGetPointDto
                {
                    NewPoint = o.NewPoint,
                    TotalPrice = o.TotalPrice,
                    OrderDate = o.OrderDate,
                    OrderId = o.OrderId,
                });
        }

        [HttpGet]
        public async Task<int> GetPoint()
        {
            var userId = _userIdentityService.GetUserId();
            var point = await _context.Users.FindAsync(userId);
            return point.Points.GetValueOrDefault();
        }
    }
}
