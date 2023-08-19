using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using TravelProject1._0.Models;
using TravelProject1._0.Models.ProductDTO;

namespace TravelProject1._0.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly TravelUserContext _db;
     
        public ProductApiController( TravelUserContext travelUserDbContext)

        {
            _db = travelUserDbContext;
        }
        public async Task <List<ProductDTO>> GetImge(int id) 
        {
            var Product = _db.Products.Select(x =>
            new ProductDTO
            {
                ProductName = x.Name,
                CategoryId = x.CategoryId,
                Price = x.Price,
                ProductId = x.Id,
                Image="/lib/image/foreast.jpeg"
            }).ToList();
            return Product;
        }
    }
}
