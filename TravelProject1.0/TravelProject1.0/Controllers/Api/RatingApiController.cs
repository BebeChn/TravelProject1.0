using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Services;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/Rating/[action]")]
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
        [HttpPost("{id}")]
        public async Task<bool> PostRating([FromBody] RatingAddDto model)
        {
            if (model == null) return false;
            int userId = _userIdentityService.GetUserId();
            try
            {
                await _dbContext.AddAsync(new Rating
                {
                    UserId = userId,
                    ProductId = model.ProductId,
                    RatingScore = model.RatingScore,
                    Describe = model.Describe,
                    RatingDate = model.RatingDate,
                });
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