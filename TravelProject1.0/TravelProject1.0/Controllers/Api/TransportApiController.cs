using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/Transport/[action]")]
    [ApiController]
    public class TransportApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;
        public TransportApiController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        //取得交通種類的商品
        [HttpGet]
        public async Task<List<TransportDto>> GetCategoryByTransport()
        {
            return await _dbContext.Products.Where(p => p.Id == 3).Select(p => new TransportDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            }).ToListAsync();
        }

        //排序商品的價格
        //低到高
        [HttpGet]
        public async Task<List<TransportDto>> TransportOrderbyPrice()
        {
            return await _dbContext.Products.Where(p => p.Id == 3).OrderBy(p => p.Price).Select(p => new TransportDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            }).ToListAsync();
        }
        //高到低
        [HttpGet]
        public async Task<List<TransportDto>> TransportOrderByDescendingPrice()
        {
            return await _dbContext.Products.Where(p => p.Id == 3).OrderByDescending(p => p.Price).Select(p => new TransportDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            }).ToListAsync();
        }

        //取得商品方案
        [HttpGet("{id}")]
        public async Task<List<TransportPlanDto>> GetPlan(int id)
        {
            return await _dbContext.Plans.Where(p => p.ProductId == id).Select(p => new TransportPlanDto
            {
                PlanId = p.PlanId,
                Name = p.Name,
                Describe = p.Describe,
                PlanImg = p.PlanImg,
                PlanPrice = p.PlanPrice
            }).ToListAsync();
        }

        //取得單一商品資訊
        [HttpGet("{id}")]
        public async Task<List<TransportPlanDto>> GetProduct(int id)
        {
            return await _dbContext.Products.Where(p => p.ProductId == id).Select(p => new TransportPlanDto
            {
                ProductName = p.ProductName,
                MainDescribe = p.MainDescribe,
                SubDescribe = p.SubDescribe,
                ShortDescribe = p.ShortDescribe,
                Img = p.Img
            }).ToListAsync();
        }

        //商品評價
        [HttpGet("{id}")]
        public async Task<List<RatingDto>> GetRating(int id)
        {
            return await _dbContext.Ratings.Include(r => r.User).Where(r => r.ProductId == id).Select(r => new RatingDto
            {
                Name = r.User.Name,
                RatingScore = r.RatingScore,
                Describe = r.Describe,
                RatingDate = r.RatingDate
            }).ToListAsync();
        }
    }
}
