using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductApiController(TravelProjectAzureContext context, IProductSearchService searchService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _searchService = searchService;
            _webHostEnvironment = webHostEnvironment;
        }

        //商品
        [HttpGet]
        public List<GetProductDTO> AdminGetProduct()
        {
            return _context.Products.AsNoTracking().Select(p => new GetProductDTO
            {
                ProductId = p.ProductId,
                Id = p.Id,
                ProductName = p.ProductName,
                MainDescribe = p.MainDescribe,
                Price = p.Price,
                SubDescribe = p.SubDescribe,
                ShortDescribe = p.ShortDescribe,
                ImagePath = string.IsNullOrEmpty(p.Img) ? "" : Path.Combine("//", p.Img)
            }).ToList();
        }

        //商品明細
        [HttpGet("{id}")]
        public async Task<ActionResult<GetProdcutDetailDTO>> GetProductDetails(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null) return NotFound();

                GetProdcutDetailDTO gpd = new GetProdcutDetailDTO
                {
                    ProductId = product.ProductId,
                    Id = product.Id,
                    ProductName = product.ProductName,
                    MainDescribe = product.MainDescribe,
                    Price = product.Price,
                    SubDescribe = product.SubDescribe,
                    ShortDescribe = product.ShortDescribe,
                    ImagePath = Path.Combine("//", product.Img)
                };

                return Ok(gpd);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        //新增商品
        [HttpPost]
        public async Task<IActionResult> AdminPostProduct([FromForm] PostProductViewModel pp)
        {
            try
            {
                var categorys = new string[] { "", "planeTK", "Books", "Transport", "Attractions" };
                var path = "";
                if (pp.File != null)
                {
                    if (pp.File.Length > 0)
                    {
                        path = @$"/images/Images.Project/{categorys[pp.Id]}/{DateTime.Now.Ticks}_{pp.File.FileName}";

                        using (var fs = new FileStream($"{_webHostEnvironment.WebRootPath}/{path}", FileMode.Create))
                        {
                            await pp.File.CopyToAsync(fs);
                        }
                    }
                }

                Product insertproduct = new Product
                {
                    Id = pp.Id,
                    ProductName = pp.ProductName,
                    Price = pp.Price,
                    MainDescribe = pp.MainDescribe,
                    SubDescribe = pp.SubDescribe,
                    ShortDescribe = pp.ShortDescribe,
                    Img = path
                };
                await _context.Products.AddAsync(insertproduct);
                await _context.SaveChangesAsync();
                return Ok(insertproduct);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        //修改商品
        [HttpPut("{id}")]
        public async Task<bool> PutProduct(int id, PutProductVIewModel ppvm)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                product.Id = ppvm.Id;
                product.ProductName = ppvm.ProductName;
                product.Price = ppvm.Price;
                product.MainDescribe = ppvm.MainDescribe;
                product.SubDescribe = ppvm.SubDescribe;
                product.ShortDescribe = ppvm.ShortDescribe;

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        //刪除
        [HttpDelete("{id}")]
        public async Task<string> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return "找不到該商品";
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

        //價格低到高
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct2(int id, [FromForm] PutProductVIewModel ppvm)
        {
            try
            {
                if (ppvm != null)
                {
                    var path = "";
                    var product = await _context.Products.FindAsync(id);
                    if (product != null)
                    {
                        if (ppvm.imageFile != null)
                        {
                            if (!string.IsNullOrEmpty(product.Img))
                            {
                                var oldImagePath = Path.Combine(@$"{_webHostEnvironment.WebRootPath}{product.Img}");
                                if (System.IO.File.Exists(oldImagePath))
                                {

                                    var categorys = new string[] { "", "planeTK", "Books", "Transport", "Attractions" };
                                    var newImagePath = @$"/images/Images.Project/{categorys[ppvm.Id]}/{DateTime.Now.Ticks}{ppvm.imageFile.FileName}";
                                    using (var fs = new FileStream($"{_webHostEnvironment.WebRootPath}{newImagePath}", FileMode.Create))
                                    {
                                        await ppvm.imageFile.CopyToAsync(fs);
                                    }
                                    System.IO.File.Delete(oldImagePath.ToString());
                                    product.Id = ppvm.Id;
                                    product.ProductName = ppvm.ProductName;
                                    product.Price = ppvm.Price;
                                    product.MainDescribe = ppvm.MainDescribe;
                                    product.SubDescribe = ppvm.SubDescribe;
                                    product.ShortDescribe = ppvm.ShortDescribe;
                                    product.Img = newImagePath;

                                    _context.Entry(product).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                    return Ok(product);
                                }
                            }
                        }
                    }
                }
                return BadRequest();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
