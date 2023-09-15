﻿using Microsoft.AspNetCore.Authorization;
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
            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Plan).Where(o => o.UserId == userId && o.OrderId==id)
                .Select(o => new OrderInfo
                {
                    OrderDate = o.OrderDate,
                    OrderId = o.OrderId,
                    Detail = o.OrderDetails.Select(z => new OrderDetailDto
                    {
                        PlanId = z.PlanId,
                        Quantity = z.Quantity,
                        UnitPrice = z.UnitPrice,
                        Odimg = z.Odimg,
                        Odname = z.Odname,
                        ProductId = z.Plan.ProductId
                    })
                });
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<UserOrderDTO> UserOrder()
        {
            var userId = _userIdentityService.GetUserId();
            return _context.Orders.Where(o => o.UserId == userId).Select(o => new UserOrderDTO
            {
                OrderDate=o.OrderDate.Value.ToString("yyyy-MM-dd"),
                OrderId = o.OrderId,
                Status = o.Status,
                UserId = userId
            });
              
               
        }
        [HttpGet]
        public IEnumerable<OrderGetPointDTO> OrderDetailsGetPoint()
        {
            var userId = _userIdentityService.GetUserId();
            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Plan).Where(o => o.UserId == userId)
                .Select(o => new OrderGetPointDTO
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
