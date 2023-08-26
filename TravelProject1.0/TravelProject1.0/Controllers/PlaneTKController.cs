using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlaneTKController : Controller
    {
        private readonly TravelProjectAzureContext _db;

        public PlaneTKController(TravelProjectAzureContext db)
        {
            _db = db;
        }







        
        public IActionResult PlaneTK_catgory()
        {
            return View();
        }





        [HttpGet]
        public async Task<IEnumerable<PlaneTKDTO>> PlaneTK_catgoryGET()
        {

            return _db.Products.Where(p => p.Id == 1)
               .Select(x => new PlaneTKDTO
               {

                   ProductId = x.ProductId,
                   Id = x.Id,
                   ProductName = x.ProductName,
                   Price = x.Price,
                   MainDescribe = x.MainDescribe,
                   SubDescribe = x.SubDescribe,
                   ShortDescribe = x.ShortDescribe,
               });
        }













































        //=======================================================
        public IActionResult PlaneTK_sale()
        {
            return View();
        }


    }
}
