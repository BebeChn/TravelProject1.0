using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Numerics;
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





        //目錄============================================


        public IActionResult PlaneTK_catgory()
        {
            return View();
        }





        //[HttpGet]
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
                   Img = x.Img,
               });
        }


        //[HttpGet]





        //
        public async Task<IEnumerable<PlaneTKDTO>> pricelower()
        {
            return _db.Products.Where(y => y.Id == 1).
                OrderBy(w => w.Price)
                .Select(x => new PlaneTKDTO
                {
                    ProductId = x.ProductId,
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    MainDescribe = x.MainDescribe,
                    SubDescribe = x.SubDescribe,
                    ShortDescribe = x.ShortDescribe,
                    Img = x.Img,

                });
        }




        //
        public async Task<IEnumerable<PlaneTKDTO>> priceheigh()
        {
            return _db.Products.Where(y => y.Id == 1).
                OrderByDescending(w => w.Price)
                .Select(x => new PlaneTKDTO
                {
                    ProductId = x.ProductId,
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    MainDescribe = x.MainDescribe,
                    SubDescribe = x.SubDescribe,
                    ShortDescribe = x.ShortDescribe,
                    Img = x.Img,


                });
        }



































        //=======================================================
        //商品
        [HttpGet]
        public IActionResult PlaneTK_sale()
        {


            return View();
        }


        [HttpGet("{id}")]
        public IActionResult PlaneTK_sale(int id)
        {


            return View();
        }

        //GET資料
        [HttpGet("{id}")]
        public async Task<IEnumerable<PlaneTKDTO>> PlaneTKGETID(int id)
        {

            return _db.Products.Where(p => p.Id == 1
                && p.ProductId == id)
                .Select(op => new PlaneTKDTO
                {
                    ProductId = op.ProductId,
                    Id = op.Id,
                    ProductName = op.ProductName,
                    Price = op.Price,
                    MainDescribe = op.MainDescribe,
                    SubDescribe = op.SubDescribe,
                    ShortDescribe = op.ShortDescribe,
                });
        }

    }
}
