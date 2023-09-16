using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TravelProject1._0.Areas.Admin.Models.DTO;
using TravelProject1._0.Areas.Admin.Models.ViewModel;
using TravelProject1._0.Models;
using TravelProject1._0.Services;

namespace TravelProject1._0.Areas.Admin.Controllers.Api
{
    [Area("Admin")]
    [Route("api/ProductApi/[action]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _context;
        private readonly IProductSearchService _searchService;
        public ProductApiController(TravelProjectAzureContext context, IProductSearchService searchService)
        {
            _context = context;
            _searchService = searchService;
        }
        [HttpGet]
        public IEnumerable<GetProductDTO> AdminGetProduct()
        {
            return _context.Products.Select(p => new GetProductDTO
            {
              ProductId = p.ProductId,
              Id = p.Id,
              ProductName = p.ProductName,
              MainDescribe = p.MainDescribe,
              Price = p.Price,
            });

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetProdcutDetailDTO>> GetProductDetails(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                GetProdcutDetailDTO gpd = new GetProdcutDetailDTO
                {
                    ProductId = product.ProductId,
                    Id = product.Id,
                    ProductName = product.ProductName,
                    MainDescribe = product.MainDescribe,
                    Price = product.Price,
                    SubDescribe = product.SubDescribe,
                    ShortDescribe = product.ShortDescribe,
                    ImagePath= Path.Combine("//", product.Img)
                };

                return Ok(gpd);
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }
        [HttpPost]
        public async Task <IActionResult> AdminPostProduct([FromBody] PostProductViewModel pp)
        {

            Product insertproduct = new Product
            {
              Id=pp.Id,
              ProductName=pp.ProductName,
              Price = pp.Price,
              MainDescribe = pp.MainDescribe,
              SubDescribe=pp.SubDescribe,
              ShortDescribe=pp.ShortDescribe,
            };
            try
            {
               await _context.Products.AddAsync(insertproduct);
               await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok(insertproduct);
        }
 
        [HttpPut("{id}")]
        public async Task<string> PutProduct(int id, PutProductVIewModel ppvm)
        {
            var product = await _context.Products.FindAsync(id);
            product.Id = ppvm.Id;
            product.ProductName = ppvm.ProductName;
            product.Price = ppvm.Price;
            product.MainDescribe = ppvm.MainDescribe;
            product.SubDescribe = ppvm.SubDescribe;
            product.ShortDescribe = ppvm.ShortDescribe;

            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return "成功";
        }
        [HttpDelete("{id}")]
        public async Task<string> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return "找不到該商品";
            }
            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return "刪除成功";
            }
            catch
            {
                return "刪除關聯失敗";
            }
        }
        [HttpGet]
        public async Task<IQueryable<ProductOrderDTO>> OrderByPrice()
        {
            return _context.Products.OrderBy(p => p.Price).Select(p => new ProductOrderDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                MainDescribe = p.MainDescribe,
                Price = p.Price,
                ProductId = p.ProductId,

            });
        }
        //價格高到低
        [HttpGet]
        public async Task<IQueryable<ProductOrderDTO>> OrderByDescendingPrice()
        {
            return _context.Products.OrderByDescending(p => p.Price).Select(p => new ProductOrderDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                MainDescribe = p.MainDescribe,
                Price = p.Price,
                ProductId = p.ProductId,
            });
        }
        [HttpGet]
        public IActionResult AdminSearchProducut(string query)
        {
            var searchResults = _searchService.SearchUsers(query);
            return Ok(searchResults);
        }

    }
}
