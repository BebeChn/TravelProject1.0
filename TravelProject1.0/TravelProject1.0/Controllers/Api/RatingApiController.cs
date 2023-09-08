using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models.ViewModel;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RatingApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;
        public RatingApiController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        //商品評價
        [HttpPost]
        public async Task<IActionResult> PostRating([FromBody] RatingDTO model)
        {
            if (model == null) return BadRequest();

            Claim user = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            string? idu = user.Value;
            int id = Convert.ToInt32(idu);

            Rating rating = new Rating
            {
                UserId = id,
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
            Claim user = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            string? idu = user.Value;
            int id = Convert.ToInt32(idu);

            return _dbContext.Plans.Include(p => p.OrderDetails).Where(p => p.ProductId == 61 && p.PlanId == 10).Select(p => new RatingInfo
            {
                PlanId = p.PlanId,
                ProductId = p.ProductId,
                Name = p.Name,
                PlanImg = p.PlanImg,
                OrderDetails = p.OrderDetails.Select(o => new OrderDetailDto
                {
                    Quantity = o.Quantity,
                    UnitPrice = o.UnitPrice,
                    UseDate = o.UseDate
                })
            });
        }
    }
}
