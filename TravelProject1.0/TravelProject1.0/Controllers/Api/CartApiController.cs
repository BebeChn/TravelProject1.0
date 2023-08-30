using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;
        public CartApiController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }  
    }
}
