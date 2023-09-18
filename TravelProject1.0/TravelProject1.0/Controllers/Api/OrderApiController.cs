using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Services;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/Order/[action]")]
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
        public async Task<List<OrderInfo>> OrderDetails(int id)
        {
            var userId = _userIdentityService.GetUserId();
            return await _context.OrderDetails.Include(od => od.Order).Include(od => od.Plan).Where(od => od.Order.UserId == userId && od.OrderId == id).Select(od => new OrderInfo
            {
                PlanId = od.PlanId,
                OrderId = od.OrderId,
                Odimg = od.Odimg,
                Odname = od.Odname,
                UseDate = od.UseDate,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                ProductId = od.Plan.ProductId
            }).ToListAsync();
        }

        [HttpGet]
        [Authorize]
        public async Task<List<UserOrderDTO>> UserOrder()
        {
            var userId = _userIdentityService.GetUserId();
            return await _context.Orders.Where(o => o.UserId == userId).Select(o => new UserOrderDTO
            {
                OrderDate = o.OrderDate.HasValue ? o.OrderDate.Value.ToString("yyyy-MM-dd") : "",
                OrderId = o.OrderId,
                Status = o.Status,
                UserId = userId
            }).ToListAsync();
        }

        [HttpGet]
        public async Task<List<OrderGetPointDTO>> OrderDetailsGetPoint()
        {
            var userId = _userIdentityService.GetUserId();
            return await _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Plan).Where(o => o.UserId == userId)
                .Select(o => new OrderGetPointDTO
                {
                    NewPoint = o.NewPoint,
                    TotalPrice = o.TotalPrice,
                    OrderDate = o.OrderDate,
                    OrderId = o.OrderId,
                }).ToListAsync();
        }

        [HttpGet]
        public async Task<int> GetPoint()
        {
            var userId = _userIdentityService.GetUserId();
            var point = await _context.Users.FindAsync(userId);
            if (point == null) return 0;
            return point.Points.GetValueOrDefault();
        }
    }
}
