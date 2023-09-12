using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models.ViewModel;
using TravelProject1._0.Services;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RatingApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;
        private readonly IUserIdentityService _userIdentityService;
        public RatingApiController(TravelProjectAzureContext dbContext, IUserIdentityService userIdentityService)
        {
            _dbContext = dbContext;
            _userIdentityService = userIdentityService;
        }

        //商品評價
        [HttpPost]
        public async Task<IActionResult> PostRating([FromBody] RatingDTO model)
        {
            if (model == null) return BadRequest();

            int userId = _userIdentityService.GetUserId();

            Rating rating = new Rating
            {
                UserId = userId,
                ProductId = model.ProductId,
                RatingScore = model.RatingScore,
                Describe = model.Describe,
                RatingDate = model.RatingDate
            };

            await _dbContext.AddAsync(rating);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok();
        }

        //取得商品評價資訊
        [HttpGet]
        public async Task<IQueryable<RatingInfo>> GetPlanInfo()
        {
            int userId = _userIdentityService.GetUserId();

            return _dbContext.OrderDetails.Select(od => new RatingInfo
            {
                PlanId = od.PlanId,
                Odimg = od.Odimg,
                Odname = od.Odname,
                UseDate = od.UseDate,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity
            });
        }
    }
}
