using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
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
        public async Task<IEnumerable<AttractionsDto>> GetCategoryByAttractions()
        {
            return _dbContext.Products.Where(c => c.Id == 4).Select(p => new AttractionsDto
            {
                ProductId = p.ProductId,
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            });
        }

        //排序
        //價格低到高
        [HttpGet]
        public async Task<IQueryable<AttractionsDto>> OrderByPrice()
        {
            return _dbContext.Products.Where(p => p.Id == 4).OrderBy(p => p.Price).Select(p => new AttractionsDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            });
        }
        //價格高到低
        [HttpGet]
        public async Task<IQueryable<AttractionsDto>> OrderByDescendingPrice()
        {
            return _dbContext.Products.Where(p => p.Id == 4).OrderByDescending(p => p.Price).Select(p => new AttractionsDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
                Img = p.Img
            });
        }

        //取得商品方案
        [HttpGet]
        [Route("{id}")]
        public async Task<IQueryable<AttractionsPlanDto>> GetPlan(int id)
        {
            return _dbContext.Plans.Where(p => p.ProductId == id).Select(p => new AttractionsPlanDto
            {
                PlanId = p.PlanId,
                Name = p.Name,
                Describe = p.Describe,
                PlanImg = p.PlanImg,
                PlanPrice = p.PlanPrice
            });
        }

        //取得單一商品資訊
        [HttpGet]
        [Route("{id}")]
        public async Task<IQueryable<AttractionsPlanDto>> GetProduct(int id)
        {
            return _dbContext.Products.Where(p => p.ProductId == id).Select(p => new AttractionsPlanDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                MainDescribe = p.MainDescribe,
                SubDescribe = p.SubDescribe,
                ShortDescribe = p.ShortDescribe,
                Img = p.Img
            });
        }

        //商品評價
        [HttpGet]
        [Route("{id}")]
        public async Task<IQueryable<RatingDto>> GetRating(int id)
        {
            return _dbContext.Ratings.Include(r => r.User).Where(r => r.ProductId == id).Select(r => new RatingDto
            {
                Name = r.User.Name,
                RatingScore = r.RatingScore,
                Describe = r.Describe,
                RatingDate = r.RatingDate
            });
        }
    }
}
