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

    [Route("api/ OrderApi/[action]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _context;
        private readonly IUserSearchService _userSearchService;
        public ProductApiController(TravelProjectAzureContext context, IUserSearchService userSearchService)
        {
            _context = context;
            _userSearchService = userSearchService;
        }
        [HttpGet]
        public IEnumerable<GetProductDTO> GetProduct()
        {
            return _context.Products.Select(x => new GetProductDTO
            {
              Id = x.Id,
              ProductName = x.ProductName,
              MainDescribe = x.MainDescribe,
              Price = x.Price,
              ProductId = x.ProductId,
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
                };

                return Ok(gpd);
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult PostProduct(PostProductViewModel pp)
        {
           

            Product insertproduct = new Product
            {
              ProductId=pp.ProductId,
              Id=pp.Id,
              ProductName=pp.ProductName,
              Price = pp.Price,
              MainDescribe = pp.MainDescribe,
              SubDescribe=pp.SubDescribe,
              ShortDescribe=pp.ShortDescribe,
            };
            try
            {
                _context.Products.AddAsync(insertproduct);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok();
        }
 
        [HttpPut("{id}")]
        public async Task<string> AdminPutUser(int id, AdminPutUserDTO apuDTO)
        {

            var admin = await _context.Users.FindAsync(id);
            admin.Name = apuDTO.Name;
            admin.Gender = apuDTO.Gender;
            admin.Email = apuDTO.Email;
            admin.Birthday = apuDTO.Birthday;
            admin.Phone = apuDTO.Phone;

            _context.Entry(admin).State = EntityState.Modified;
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
        public async Task<string> AdminDeleteUser(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return "找不到該用戶";
            }
            try
            {
                _context.Users.Remove(users);
                await _context.SaveChangesAsync();
                return "刪除成功";
            }
            catch
            {
                return "刪除關聯失敗";
            }
        }
        [HttpGet]
        public async Task<IQueryable<AdminGetUserViewModel>> OrderByAge()
        {
            return _context.Users.OrderBy(u => u.Age).Select(u => new AdminGetUserViewModel
            {
                UserId = u.UserId,
                Email = u.Email,
                Gender = u.Gender,
                Name = u.Name,
                Phone = u.Phone,
                Birthday = u.Birthday.Value.ToString("yyyy-MM-dd"),

            });
        }
        //價格高到低
        [HttpGet]
        public async Task<IQueryable<AdminGetUserViewModel>> OrderByDescendingAge()
        {
            return _context.Users.OrderByDescending(u => u.Age).Select(u => new AdminGetUserViewModel
            {
                UserId = u.UserId,
                Email = u.Email,
                Gender = u.Gender,
                Name = u.Name,
                Phone = u.Phone,
                Birthday = u.Birthday.Value.ToString("yyyy-MM-dd"),
            });
        }
        [HttpGet]
        public IActionResult AdminSearchUser(string query)
        {
            var searchResults = _userSearchService.SearchUsers(query);
            return Ok(searchResults);
        }

    }
}
