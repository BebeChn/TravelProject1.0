using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    public class BookApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;

        public BookApiController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        //取得住宿資料
        [HttpGet]
        public IEnumerable<BookDTO> GetBooks()
        {
            return _dbContext.Products.Where(b => b.Id == 2).Select(b => new BookDTO
            {
                ProductId = b.ProductId,
                ProductName = b.ProductName,
                Price = b.Price,
                MainDescribe = b.MainDescribe,
                Img = b.Img
            });
        }

        //商品價格排序低到高
        [HttpGet]
        public IEnumerable<BookDTO> BookOrderByPrice() 
        {
            return _dbContext.Products.Where(b => b.Id == 2).OrderBy(b => b.Price).Select(b => new BookDTO
            {
                ProductId=b.ProductId,
                ProductName=b.ProductName,
                Price = b.Price,
                MainDescribe = b.MainDescribe,
                Img = b.Img
            });
        }
        //商品價格排序高到低
        [HttpGet]
        public IEnumerable<BookDTO> BookOrderByDescendingPrice() 
        {
            return _dbContext.Products.Where(b => b.Id == 2).OrderByDescending(b => b.Price).Select(b => new BookDTO
            {
                ProductId= b.ProductId,
                ProductName=b.ProductName,
                Price = b.Price,
                MainDescribe = b.MainDescribe,
                Img = b.Img
            });
        }

        //取得單一商品
        [HttpGet]
        [Route("{id}")]
        public IEnumerable<BookPlanDTO> GetProduct(int id)
        {
            return _dbContext.Products.Where(b => b.ProductId == id).Select(b => new BookPlanDTO
            {
                ProductId = b.ProductId,
                ProductName = b.ProductName,
                MainDescribe = b.MainDescribe,
                ShortDescribe = b.ShortDescribe,
                SubDescribe = b.SubDescribe,
                Img = b.Img
            });
        }

        //取得商品的方案
        [HttpGet]
        [Route("{id}")]
        public IEnumerable<BookPlanDTO> GetPlan(int id)
        {
            return _dbContext.Plans.Where(c => c.ProductId == id).Select(c => new BookPlanDTO
            {
                PlanId = c.PlanId,
                Name = c.Name,
                Describe = c.Describe,
                PlanImg = c.PlanImg,
                PlanPrice = c.PlanPrice,
            });
        }

        //商品評價
        [HttpGet]
        [Route("{id}")]
        public async Task<IQueryable<RatingDTO>> GetRating(int id)
        {
            return _dbContext.Ratings.Include(r => r.User).Where(r => r.ProductId == id).Select(r => new RatingDTO
            {
                Name = r.User.Name,
                RatingScore = r.RatingScore,
                Describe = r.Describe,
                RatingDate = r.RatingDate
            });
        }
    }
}
