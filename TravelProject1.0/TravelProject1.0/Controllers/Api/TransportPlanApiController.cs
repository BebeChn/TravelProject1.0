using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class TransportPlanApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;
        public TransportPlanApiController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        //取得交通商品ID查詢商品方案
        [HttpGet]
        public async Task<IEnumerable<TransportPlanDTO>> GetPlanByTransport(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null) return null;

            TransportPlanDTO transportPlanDTO = new TransportPlanDTO
            {

                ProductId = product.Id,
                ProductName = product.ProductName,
                MainDescribe = product.MainDescribe,
                SubDescribe = product.SubDescribe,
                ShortDescribe = product.ShortDescribe
            };

            var result = _dbContext.Plans.Where(p => p.PlanId == id).Select(p => new TransportPlanDTO
            {
                PlanId = p.PlanId,
                Name = p.Name,
                Describe = p.Describe
            });

            return result;
        }
    }
}
