using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using NToastNotify.Helpers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelProject1._0.Areas.Admin.Models.DTO;
using TravelProject1._0.Areas.Admin.Models.ViewModel;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Services;

namespace TravelProject1._0.Areas.Admin.Controllers.Api
{
    [Area("Admin")]
    [Route("api/AdminApi/[action]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _context;
        private readonly IUserSearchService _userSearchService;
        public AdminApiController(TravelProjectAzureContext context, IUserSearchService userSearchService)
        {
            _context = context;
            _userSearchService = userSearchService;
        }

        [HttpGet]
        public IEnumerable<AdminGetUserViewModel> AdminGetUser()
        {
            return _context.Users.Select(x => new AdminGetUserViewModel

            {
                UserId = x.UserId,
                Email = x.Email,
                Gender = x.Gender,
                Name = x.Name,
                Phone = x.Phone,
                Birthday = x.Birthday.Value.ToString("yyyy-MM-dd"),
            });

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDetailDTO>> GetUserDetail(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                GetUserDetailDTO advm = new GetUserDetailDTO
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Gender = user.Gender,
                    Name = user.Name,
                    Phone = user.Phone,
                    Birthday = user.Birthday?.ToString("yyyy-MM-dd HH:mm:ss")
                };

                return Ok(advm);
            }
            catch (Exception ex)
            {

                return null;
            }
        }
            
        [HttpPost]
        public IActionResult AdminManageUser(AdmminManageUserDTO amuDTO)
        {
            var salt = GenerateSalt();

            var passwordhash = HashPassword(amuDTO.Password, salt);

            User insertuser = new User
            {
                Name = amuDTO.Name,
                Email = amuDTO.Email,
                PasswordHash = passwordhash,
                Gender = amuDTO.Gender,
                Phone = amuDTO.Phone,
                Birthday = amuDTO.Birthday,
            };
            try
            {
                _context.Users.AddAsync(insertuser);
                _context.SaveChanges();
             }
            catch (Exception ex)
            {
                return BadRequest();
            }

            //List<Claim> claims = new List<Claim>();
            //claims.Add(new Claim(ClaimTypes.NameIdentifier, amuDTO.Id.ToString()));
            //claims.Add(new Claim(ClaimTypes.Name, $"{amuDTO.Name}"));
            //claims.Add(new Claim("Email", amuDTO.Email));
            //claims.Add(new Claim(ClaimTypes.Role, "user"));
            //ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            return Ok();
        }
        private string GenerateSalt()
        {
            Byte[] bytes = new Byte[16];
            using (var ran = RandomNumberGenerator.Create())
            {
                ran.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }
        private string HashPassword(string paswword, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                string paswwordsalt = paswword + salt;
                byte[] passwordhash = Encoding.UTF8.GetBytes(paswwordsalt);
                byte[] psaswordhash = sha256.ComputeHash(passwordhash);
                return Convert.ToBase64String(psaswordhash);
            }
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
