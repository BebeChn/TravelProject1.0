using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models.ProductDTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/Product/[action]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _db;
        public ProductApiController(TravelProjectAzureContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<List<ProductDTO>> GetProduct()
        {
            return await _db.Products.Where(x => x.Id == 4).Select(x => new ProductDTO
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
                Img = x.Img
            }).Take(4).ToListAsync();
        }

        [HttpGet]
        public async Task<List<ProductPlanDto>> GetProductPlan()
        {
            return await _db.Products.Where(x => x.Id == 2).Select(x => new ProductPlanDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Img = x.Img
            }).Take(3).ToListAsync();
        }

        [HttpGet]
        public async Task<List<AttractionDto>> GetAttractionPlan()
        {
            return await _db.Products.Where(x => x.Id == 3).Select(x => new AttractionDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Img = x.Img,
                MainDescribe = x.MainDescribe,
            }).Take(1).ToListAsync();
        }
    }
}