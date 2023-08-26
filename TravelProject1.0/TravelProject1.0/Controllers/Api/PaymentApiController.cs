using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContent;
        public PaymentApiController(TravelProjectAzureContext dbContent)
        {
            _dbContent = dbContent;
        }

        public async Task<IQueryable<Plan>> GetPlan(int id)
        {
            
        }
    }
}
