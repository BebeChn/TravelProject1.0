using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/UserApi/[Action]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;

        public UserApiController(ILogger<HomeController> logger, TravelProjectAzureContext context)

        {
            _logger = logger;
            _context = context;
          
        }
        // GET: api/<UserApiController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult>getpeople()
        {
            IQueryable<User> userQry = _context.Users;
            UserDTO[] user=await userQry
                .Select(u => new UserDTO
                {
                    Name=u.Name,
                    Email = u.Email,
                    Gender = u.Gender,
                    Birthday=u.Birthday, 
                    Phone = u.Phone,
                })
                .ToArrayAsync();

            return Ok(user);
        }
        // POST api/<UserApiController>
        [HttpPost]
        public async Task<IActionResult>postpeople(RegisterDTO register)
        {
            // 檢查用戶名與用法是否為空
            if (string.IsNullOrEmpty(register.Name) || string.IsNullOrEmpty(register.Password))
            {
                return BadRequest("帳號或密碼已被使用"); 
            }
            // 對密碼進行加鹽
            string salt = GenerateSalt();
   
            string hashedPassword = HashPassword(register.Password, salt);

            // 創建用戶實體
            User newUser = new User
            {
                Name = register.Name,
                Gender = register.Gender,
                Email = register.Email,
                Birthday = register.Birthday,
                Password = register.Password,
                PasswordHash = hashedPassword,
                Salt=salt,
               
            };

            // 添加用戶到資料庫
            try { 
            _context.Users.Add(newUser);
            }
            catch { 
            _context.SaveChanges();
                }
          
            List<Claim> claims = new List<Claim>();
            new Claim(ClaimTypes.Name, $"{register.Name}");
            new Claim("Email", register.Email);
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
            return RedirectToAction("Index", "Home");
        }
        // 生成隨機鹽
        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var ran = RandomNumberGenerator.Create())
            {
                ran.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        // 使用SHA-256哈希密碼並加鹽
        private string HashPassword(string password, string salt)
        {
            using (var SHA256 = SHA256Managed.Create())
            {
                // 將密碼轉換成二進位
                string passwordWithSalt = password + salt;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(passwordWithSalt);
                // 計算密碼哈希
                byte[] hashBytes = SHA256.ComputeHash(passwordBytes);
                // 將密碼哈希轉換為Base64编碼的字串
                return Convert.ToBase64String(hashBytes);
            }
        }


        // PUT api/<UserApiController>/5
        [HttpPut("{id}")]
        public void PutPeople(int id, [FromBody] string value)
        {

        }

       
    }
}
