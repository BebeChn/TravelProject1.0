using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using TravelProject1._0.Models;
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
    }
}
