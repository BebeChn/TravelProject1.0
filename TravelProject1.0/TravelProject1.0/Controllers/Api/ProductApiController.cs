using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models.ProductDTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _db;
        public ProductApiController(TravelProjectAzureContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IQueryable<ProductDTO>> GetProduct()
        {
            return _db.Products.Where(x => x.Id == 4).Select(x => new ProductDTO
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
                Img = x.Img
            }).Take(4);
        }
        [HttpGet]
        public async Task<IQueryable<ProductPlanDTO>> GetProductPlan()
        {
            return _db.Products.Where(x => x.Id == 2).Select(x => new ProductPlanDTO
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Img = x.Img
            }).Take(3);
        }
        [HttpGet]
        public IQueryable<AttractionDTO> GetAttractionPlan()
        {
            return  _db.Products.Where(x => x.Id == 3).Select(x => new AttractionDTO
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Img = x.Img,
                MainDescribe = x.MainDescribe,

            }).Take(1);
        }
    }
}

