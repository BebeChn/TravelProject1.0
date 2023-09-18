using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/Book/[action]")]
    public class BookApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;
        public BookApiController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        //取得住宿資料
        [HttpGet]
        public async Task<List<BookDTO>> GetBooks()
        {
            return await _dbContext.Products.Where(b => b.Id == 2).Select(b => new BookDTO
            {
                ProductId = b.ProductId,
                ProductName = b.ProductName,
                Price = b.Price,
                MainDescribe = b.MainDescribe,
                Img = b.Img
            }).ToListAsync();
        }

        //商品價格排序低到高
        [HttpGet]
        public async Task<List<BookDTO>> BookOrderByPrice()
        {
            return await _dbContext.Products.Where(b => b.Id == 2).OrderBy(b => b.Price).Select(b => new BookDTO
            {
                ProductId = b.ProductId,
                ProductName = b.ProductName,
                Price = b.Price,
                MainDescribe = b.MainDescribe,
                Img = b.Img
            }).ToListAsync();
        }

        //商品價格排序高到低
        [HttpGet]
        public async Task<List<BookDTO>> BookOrderByDescendingPrice()
        {
            return await _dbContext.Products.Where(b => b.Id == 2).OrderByDescending(b => b.Price).Select(b => new BookDTO
            {
                ProductId = b.ProductId,
                ProductName = b.ProductName,
                Price = b.Price,
                MainDescribe = b.MainDescribe,
                Img = b.Img
            }).ToListAsync();
        }

        //取得單一商品
        [HttpGet("{id}")]
        public async Task<List<BookPlanDTO>> GetProduct(int id)
        {
            return await _dbContext.Products.Where(b => b.ProductId == id).Select(b => new BookPlanDTO
            {
                ProductId = b.ProductId,
                ProductName = b.ProductName,
                MainDescribe = b.MainDescribe,
                ShortDescribe = b.ShortDescribe,
                SubDescribe = b.SubDescribe,
                Img = b.Img
            }).ToListAsync();
        }

        //取得商品的方案
        [HttpGet("{id}")]
        public async Task<List<BookPlanDTO>> GetPlan(int id)
        {
            return await _dbContext.Plans.Where(c => c.ProductId == id).Select(c => new BookPlanDTO
            {
                PlanId = c.PlanId,
                Name = c.Name,
                Describe = c.Describe,
                PlanImg = c.PlanImg,
                PlanPrice = c.PlanPrice,
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
