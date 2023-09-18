using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelProject1._0.Areas.Admin.Models.DTO;

namespace TravelProject1._0.Areas.Admin.Controllers.Api
{
	[Area("Admin")]
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SuperApiController : ControllerBase
	{
		private readonly TravelProjectAzureContext _context;

		public SuperApiController(TravelProjectAzureContext context)
		{
			_context = context;
		}
        [HttpGet]
        public IEnumerable<GetAdminDTO> GetAdmin()
        {
            return _context.Admins.Select(a => new GetAdminDTO
            {
               Id= a.Id,
               Name= a.Name,
               Account=a.Account,
               Describe=a.Describe,
               CreateDate= DateTime.Now,
               LoginDate= DateTime.Now,
            });

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAdminDetailDTO>> GetAdminDetail(int id)
        {
            try
            {
                var admin = await _context.Admins.FindAsync(id);

                if (admin == null)
                {
                    return NotFound();
                }

                GetAdminDetailDTO gad = new GetAdminDetailDTO
                {
                    Id = admin.Id,
                    Name = admin.Name,
                    Account = admin.Account,
                    Describe = admin.Describe,
                    CreateDate = DateTime.Now,
                    LoginDate = DateTime.Now,
                };

                return Ok(gad);
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult AdminManage(ManageAdmminDTO maDTO)
        {
            if (maDTO == null) return BadRequest();
            try
            {
                var salt = GenerateSalt();

            var passwordhash = HashPassword(maDTO.Password, salt);

            TravelProject1._0.Models.Admin insertadmin = new TravelProject1._0.Models.Admin
            {
              Id=maDTO.Id,
              Name=maDTO.Name,
              Account=maDTO.Account,
              Password=passwordhash,
              Describe=maDTO.Describe,
              LoginDate=DateTime.Now,
              CreateDate=DateTime.Now,
              Role="Admin",
            };
                _context.Admins.AddAsync(insertadmin);
                _context.SaveChanges();
                return Ok(insertadmin);
            }
            catch
            {
                return BadRequest();
            }
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
        public async Task<string> AdminPut(int id,[FromBody] AdminPutDTO apDTO)
        {
            var admin = await _context.Admins.FindAsync(id);
            admin.Name = apDTO.Name;
            admin.Account = apDTO.Account;
            admin.Describe = apDTO.Describe;
            admin.Password = apDTO.Password;
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
        public async Task<string> AdminDelete(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return "找不到該用戶";
            }
            try
            {
                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();
                return "刪除成功";
            }
            catch
            {
                return "刪除失敗";
            }
        }
        [HttpGet]
        public IQueryable<AdminGetUserDTO> OrderByAge()
        {
            return _context.Users.OrderBy(u => u.Age).Select(u => new AdminGetUserDTO
            {
                UserId = u.UserId,
                Email = u.Email,
                Gender = u.Gender,
                Name = u.Name,
                Phone = u.Phone,
                Birthday = u.Birthday.Value.ToString("yyyy-MM-dd"),

            });
        }
        //排序年紀
        [HttpGet]
        public IQueryable<AdminGetUserDTO> OrderByDescendingAge()
        {
            return _context.Users.OrderByDescending(u => u.Age).Select(u => new AdminGetUserDTO
            {
                UserId = u.UserId,
                Email = u.Email,
                Gender = u.Gender,
                Name = u.Name,
                Phone = u.Phone,
                Birthday = u.Birthday.Value.ToString("yyyy-MM-dd"),
            });
        }
        //[HttpGet]
        //public IActionResult AdminSearchUser(string query)
        //{
        //    var searchResults = _userSearchService.SearchUsers(query);
        //    return Ok(searchResults);
        //}

    }
}
