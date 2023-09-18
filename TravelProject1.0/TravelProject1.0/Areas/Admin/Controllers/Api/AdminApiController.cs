using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Areas.Admin.Models.DTO;
using TravelProject1._0.Models;
using TravelProject1._0.Services;

namespace TravelProject1._0.Areas.Admin.Controllers.Api
{
    [Area("Admin")]
    [Route("api/Admin/[action]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _context;
        private readonly IUserSearchService _userSearchService;
        private readonly IUserIdentityService _userIdentityService;
        public AdminApiController(TravelProjectAzureContext context, IUserSearchService userSearchService, IUserIdentityService userIdentityService)
        {
            _context = context;
            _userSearchService = userSearchService;
            _userIdentityService = userIdentityService;
        }

        [HttpGet]
        public IEnumerable<AdminGetUserDTO> AdminGetUser()
        {
            return _context.Users.Select(x => new AdminGetUserDTO

            {
                UserId = x.UserId,
                Email = x.Email,
                Gender = x.Gender,
                Name = x.Name,
                Phone = x.Phone,
                Birthday = x.Birthday.HasValue ? x.Birthday.Value.ToString("yyyy-MM-dd") : "",
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

                GetUserDetailDTO gud = new GetUserDetailDTO
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Gender = user.Gender,
                    Name = user.Name,
                    Phone = user.Phone,
                    Birthday = user.Birthday?.ToString("yyyy-MM-dd HH:mm:ss")
                };

                return Ok(gud);
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }

        [HttpPost]
        public bool AdminManageUser(AdmminManageUserDTO amuDTO)
        {
            if (amuDTO == null) return false;
            try
            {
                _context.Users.AddAsync(new User
                {
                    Name = amuDTO.Name,
                    Email = amuDTO.Email,
                    PasswordHash = amuDTO.Password,
                    Gender = amuDTO.Gender,
                    Phone = amuDTO.Phone,
                    Birthday = amuDTO.Birthday,
                });
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPut("{id}")]
        public async Task<bool> AdminPutUser(int id, AdminPutUserDTO apuDTO)
        {
            try
            {
                var admin = await _context.Users.FindAsync(id);
                if (admin == null) return false;
                admin.Name = apuDTO.Name;
                admin.Gender = apuDTO.Gender;
                admin.Email = apuDTO.Email;
                admin.Birthday = apuDTO.Birthday;
                admin.Phone = apuDTO.Phone;

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
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
        public async Task<List<AdminGetUserDTO>> OrderByAge()
        {
            return await _context.Users.OrderBy(u => u.Age).Select(u => new AdminGetUserDTO
            {
                UserId = u.UserId,
                Email = u.Email,
                Gender = u.Gender,
                Name = u.Name,
                Phone = u.Phone,
                Birthday = u.Birthday.HasValue ? u.Birthday.Value.ToString("yyyy-MM-dd") : "",
            }).ToListAsync();
        }
        //排序年紀
        [HttpGet]
        public async Task<List<AdminGetUserDTO>> OrderByDescendingAge()
        {
            return await _context.Users.OrderByDescending(u => u.Age).Select(u => new AdminGetUserDTO
            {
                UserId = u.UserId,
                Email = u.Email,
                Gender = u.Gender,
                Name = u.Name,
                Phone = u.Phone,
                Birthday = u.Birthday.HasValue ? u.Birthday.Value.ToString("yyyy-MM-dd") : "",
            }).ToListAsync();
        }
        [HttpGet]
        public IActionResult AdminSearchUser(string query)
        {
            var searchResults = _userSearchService.SearchUsers(query);
            return Ok(searchResults);
        }

    }
}
