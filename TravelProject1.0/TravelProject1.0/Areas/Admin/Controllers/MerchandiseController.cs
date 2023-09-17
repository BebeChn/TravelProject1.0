using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq;
using TravelProject1._0.Areas.Admin.Models.DTO;
using TravelProject1._0.Models;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MerchandiseController : Controller
    {
        private readonly TravelProjectAzureContext _db;
        public MerchandiseController(TravelProjectAzureContext db)
        {
            _db = db;
        }

        public IActionResult Merchandise()
        {
            return View();
        }

        [HttpGet]
        public async Task<IEnumerable<MerchandiseDTO>> MerchandiseGET()
        {
            return _db.Products.Select(x => new MerchandiseDTO
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

        [HttpPost]
        public async Task<IEnumerable<MerchandiseDTO>> MerchandiseSearch(MerchandiseDTO MchDTO)
        {
            return _db.Products.Where(y =>
            y.ProductId == MchDTO.ProductId ||
            y.Id == MchDTO.Id ||
            y.ProductName.Contains(MchDTO.ProductName) ||
            y.Price == MchDTO.Price ||
            y.MainDescribe.Contains(MchDTO.MainDescribe) ||
            y.SubDescribe.Contains(MchDTO.SubDescribe) ||
            y.ShortDescribe.Contains(MchDTO.ShortDescribe)
            ).Select(x => new MerchandiseDTO
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

        [HttpPut("{id}")]
        public async Task<string> MerchandisePUT(int id, MerchandiseDTO MchDTO)
        {
            if (id != MchDTO.ProductId)
            {
                return "修改失敗";
            }
            var Mch = await _db.Products.FindAsync(id);
            Mch.Id = MchDTO.Id;
            Mch.ProductName = MchDTO.ProductName;
            Mch.ProductName = MchDTO.ProductName;
            Mch.MainDescribe = MchDTO.MainDescribe;
            Mch.SubDescribe = MchDTO.SubDescribe;
            Mch.ShortDescribe = MchDTO.ShortDescribe;
            _db.Entry(Mch).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }
            return "成功";
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<string> MerchandiseDELETE(int id)
        {
            if (_db.Products == null)
            {
                return "刪除失敗";
            }
            var Mch = await _db.Products.FindAsync(id);
            if (Mch == null)
            {
                return "刪除失敗";
            }
            try
            {
                _db.Products.Remove(Mch);
                await _db.SaveChangesAsync();
            }
            catch
            {
                return "刪除關聯失敗";
            }
            return "刪除成功";
        }

        //insert
        [HttpPost]
        public async Task<string> MerchandiseADD(MerchandiseDTO MchDTO)
        {
            if (_db.Products == null)
            {
                return null;
            }
            Product Mch = new Product
            {
                ProductId = MchDTO.ProductId,
                Id = MchDTO.Id,
                ProductName = MchDTO.ProductName,
                Price = MchDTO.Price,
                MainDescribe = MchDTO.MainDescribe,
                SubDescribe = MchDTO.SubDescribe,
                ShortDescribe = MchDTO.ShortDescribe,
            };
            _db.Products.Add(Mch);
            await _db.SaveChangesAsync();
            MchDTO.ProductId = Mch.ProductId;
            return "成功";
        }

        //id
        [HttpGet("{id}")]
        public async Task<IEnumerable<MerchandiseDTO>> MerchandiseOption(int id)
        {
            return _db.Products.Where(p => p.Id == id)
                .Select(op => new MerchandiseDTO
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

        //lc
        [HttpGet("{lc}")]
        public async Task<IEnumerable<MerchandiseDTO>> Merchandiselocation(string lc)
        {
            return _db.Products.Where(p => p.ProductName.Contains(lc))
                .Select(op => new MerchandiseDTO
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

        [HttpGet]
        //LH
        public async Task<IEnumerable<MerchandiseDTO>> pricelower()
        {
            return _db.Products.OrderBy(y => y.Price)
                .Select(z => new MerchandiseDTO
                {
                    ProductId = z.ProductId,
                    Id = z.Id,
                    ProductName = z.ProductName,
                    Price = z.Price,
                    MainDescribe = z.MainDescribe,
                    SubDescribe = z.SubDescribe,
                    ShortDescribe = z.ShortDescribe,

                });
        }

        //HL
        public async Task<IEnumerable<MerchandiseDTO>> pricehigh()
        {
            return _db.Products.OrderByDescending(y => y.Price)
                .Select(z => new MerchandiseDTO
                {
                    ProductId = z.ProductId,
                    Id = z.Id,
                    Price = z.Price,
                    MainDescribe = z.MainDescribe,
                    SubDescribe = z.SubDescribe,
                    ShortDescribe = z.ShortDescribe,
                });
        }
    }
}
