using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models;
using Microsoft.AspNetCore.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/UserApi/[Action]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailSender _emailSender;
        public UserApiController(ILogger<HomeController> logger, TravelProjectAzureContext context, EmailSender emailSender)

        {
            _logger = logger;
            _context = context;
            _emailSender= emailSender;
          
        }
        // GET: api/<UserApiController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<UserApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult>getpeople()
        {
            //IQueryable<User> userQry = _context.Users;
            //UserDTO[] user=await userQry
            //    .Select(u => new UserDTO
            //    {
            //        Name=u.Name,
            //        Email = u.Email,
            //        Gender = u.Gender,
            //        Birthday=u.Birthday, 
            //        Phone = u.Phone,
            //    })
            //    .ToArrayAsync();

            return Ok();
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
                Phone = register.Phone,
                Password = register.Password,
                PasswordHash = hashedPassword,
                Salt = salt,
                CreateDate= DateTime.Now,
                Address=register.Address
                
            };
           
        // 添加用戶到資料庫

        _context.Users.Add(newUser);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
                return Conflict("資料庫更新失敗");
            }
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, $"{register.Name}")); 
            claims.Add(new Claim("Email", register.Email));
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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
        //[HttpPost]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        //{
        //    if (string.IsNullOrEmpty(resetPasswordDTO.Email))
        //    {
        //        return BadRequest("Email為必填");
        //    }

        //    string resetToken = Guid.NewGuid().ToString();
        //    DateTime expirationTime = DateTime.Now.AddHours(24);
        //    PasswordResetToken tokenEntity = new PasswordResetToken
        //    {
        //        Email = resetPasswordDTO.Email,
        //        Token = resetToken,
        //        ExpirationTime = expirationTime
        //    };

            
        //    _context.SaveChanges();

        //    string resetLink = $"https://yourapp.com/reset-password?token={resetToken}";


        //    await _emailSender.SendEmailAsync(resetPasswordDTO.Email, "密碼重設", $"確認連結密碼:{{resetLink}}");


        //    return Ok(new { Message = "密碼重設" });

        //}
    }



}
