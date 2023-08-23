using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
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
        public async Task<IEnumerable<AttractionsDTO>> GetCategoryByAttractions()
        {
            return _dbContext.Products.Where(c => c.Id == 4).Select(p => new AttractionsDTO
            {
                ProductId = p.ProductId,
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                MainDescribe = p.MainDescribe
            });
        }
    }
}
