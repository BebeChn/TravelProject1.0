using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
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





        //============================================就是商品目錄

        //商品目錄頁
        public IActionResult PlaneTK_catgory()
        {
            return View();
        }





        //[HttpGet]
        //取得商品
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


        





        //金額低到高
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




        //金額高到低
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



































        //=======================================================plan   商品本身
        //商品頁
        [HttpGet]
        public IActionResult PlaneTK_sale()
        {


            return View();
        }

        //給商品目錄頁用
        [HttpGet("{id}")]
        public IActionResult PlaneTK_sale(int id)
        {


            return View();
        }

        //GET Product資料
        //取得商品
        [HttpGet("{id}")]
        public async Task<IEnumerable<PlaneTKDTO>> PlaneTK_Product(int id)
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

        //GET plan資料
        //取得商品方案
        //IQueryable就是顯示一筆紀錄  IEnumerable就是一堆紀錄 集合
        [HttpGet]
        [Route("{id}")]
        public async Task<IQueryable<PlaneTK_salePlanDTO>> PlaneTK_salePlan(int id)
        {
            return _db.Plans.Where(p => p.ProductId == id)
                .Select(x => new PlaneTK_salePlanDTO
            {
                PlanId = x.PlanId,
                ProductId = x.ProductId,
                Name = x.Name,
                Describe = x.Describe,
                PlanImg = x.PlanImg,
                PlanPrice = x.PlanPrice
            });
        }

    }
}
