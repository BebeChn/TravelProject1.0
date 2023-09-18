using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/Attractions/[action]")]
    [ApiController]
    public class AttractionsApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;
        public AttractionsApiController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        //取得種類是景點的商品
        [HttpGet]
        public async Task<List<AttractionsDTO>> GetCategoryByAttractions()
        {
            return await _dbContext.Products.Where(c => c.Id == 4).Select(p => new AttractionsDTO
            {
                ProductId = p.ProductId,
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            }).ToListAsync();
        }

        //排序
        //價格低到高
        [HttpGet]
        public async Task<List<AttractionsDTO>> OrderByPrice()
        {
            return await _dbContext.Products.Where(p => p.Id == 4).OrderBy(p => p.Price).Select(p => new AttractionsDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            }).ToListAsync();
        }
        //價格高到低
        [HttpGet]
        public async Task<List<AttractionsDTO>> OrderByDescendingPrice()
        {
            return await _dbContext.Products.Where(p => p.Id == 4).OrderByDescending(p => p.Price).Select(p => new AttractionsDTO
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
        public async Task<List<AttractionsPlanDTO>> GetPlan(int id)
        {
            return await _dbContext.Plans.Where(p => p.ProductId == id).Select(p => new AttractionsPlanDTO
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
        public async Task<List<AttractionsPlanDTO>> GetProduct(int id)
        {
            return await _dbContext.Products.Where(p => p.ProductId == id).Select(p => new AttractionsPlanDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                MainDescribe = p.MainDescribe,
                SubDescribe = p.SubDescribe,
                ShortDescribe = p.ShortDescribe,
                Img = p.Img
            }).ToListAsync();
        }

        //商品評價
        [HttpGet("{id}")]
        public async Task<List<RatingDTO>> GetRating(int id)
        {
            return await _dbContext.Ratings.Include(r => r.User).Where(r => r.ProductId == id).Select(r => new RatingDTO
            {
                Name = r.User.Name,
                RatingScore = r.RatingScore,
                Describe = r.Describe,
                RatingDate = r.RatingDate
            }).ToListAsync();
        }
    }
}
