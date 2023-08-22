using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransportApiController : ControllerBase
    {
        private readonly TravelProjectContext _dbContext;

        public TransportApiController(TravelProjectContext dbContext)
        {
            _dbContext = dbContext;
        }

        //取得交通種類的商品
        [HttpGet]
        public async Task<IEnumerable<TransportDTO>> GetCategoryByTransport()
        {
            return _dbContext.Products.Where(c => c.Id == 3).Select(p => new TransportDTO
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                MainDescribe = p.MainDescribe,
            });
        }

        //排序商品的價格
        //低到高
        public async Task<IEnumerable<TransportDTO>> TransportOrderbyPrice()
        {
            return _dbContext.Products.Where(w => w.Id == 3).OrderBy(o => o.Price).Select(p => new TransportDTO
            {
                Name = p.Name,
                Price = p.Price,
                MainDescribe = p.MainDescribe
            });
        }
        //高到低
        public async Task<IEnumerable<TransportDTO>> TransportOrderByDescendingPrice()
        {
            return _dbContext.Products.Where(w => w.Id == 3).OrderByDescending(o => o.Price).Select(p => new TransportDTO
            {
                Name = p.Name,
                Price = p.Price,
                MainDescribe = p.MainDescribe
            });
        }
    }
}
