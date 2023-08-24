using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify.Helpers;
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

        public async Task<IQueryable<TransportPlanDTO>> GetPlan(int id)
        {
            return _dbContext.Plans.Where(p => p.ProductId == id).Select(p => new TransportPlanDTO
            {
                Name = p.Name,
                Describe = p.Describe,
            });
        }

        public async Task<IQueryable<TransportPlanDTO>> GetProduct()
        {
            return _dbContext.Products.Where(p => p.ProductId == 41).Select(p => new TransportPlanDTO
            {
                ProductName = p.ProductName,
                MainDescribe = p.MainDescribe,
                SubDescribe = p.SubDescribe,
                ShortDescribe = p.ShortDescribe
            });
        }
    }
}
