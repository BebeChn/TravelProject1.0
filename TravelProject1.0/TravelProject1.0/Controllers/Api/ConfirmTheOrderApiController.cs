﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models.ViewModel;
using TravelProject1._0.Services;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfirmTheOrderApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;
        private readonly IUserIdentityService _userIdentityService;
        public ConfirmTheOrderApiController(TravelProjectAzureContext dbContext, IUserIdentityService userIdentityService)
        {
            _dbContext = dbContext;
            _userIdentityService = userIdentityService;
        }

        //取得聯絡人資訊
        [HttpGet]
        public async Task<IQueryable<ConfirmTheOrderDto>> GetUser()
        {
            int userId = _userIdentityService.GetUserId();

            return _dbContext.Users.Where(u => u.UserId == userId).Select(u => new ConfirmTheOrderDto
            {
                UserId = userId,
                Name = u.Name,
                Phone = u.Phone,
                Email = u.Email,
                Points = u.Points
            });
        }

        //取得訂單資訊
        [HttpGet]
        [Route("{orderId}")]
        public async Task<IQueryable<ConfirmTheOrderViewModel>> GetOrders(int orderId)
        {
            int userId = _userIdentityService.GetUserId();

            return _dbContext.Orders.Include(o => o.OrderDetails).Where(o => o.UserId == userId && o.OrderId == orderId).Select(o => new ConfirmTheOrderViewModel
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                ConfirmTheOrderDetails = o.OrderDetails.Select(od => new ConfirmTheOrderDetailDto
                {
                    Odname = od.Odname,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                })
            });
        }
    }
}
