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
using TravelProject1._0.Models.ViewModel;
using Microsoft.Win32;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/UserApi/[Action]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;
        private readonly ConcurrentDictionary<string, VerificationCodeData> _verificationCodes = new ConcurrentDictionary<string, VerificationCodeData>();
        private readonly IEmailSender _emailSender;
        public UserApiController(ILogger<HomeController> logger, TravelProjectAzureContext context,  IEmailSender emailSender)

        {
            _logger = logger;
            _context = context;
            _verificationCodes = new ConcurrentDictionary<string, VerificationCodeData>();
            _emailSender = emailSender;

        }
        // GET: api/<UserApiController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<UserDTO> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return null;
            }
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return null;
            }
            UserDTO userDTO = new UserDTO
            {
                Name = users.Name,
                Email = users.Email,
                Birthday = users.Birthday,
                Gender = users.Gender,
                Phone = users.Phone,
            };
            return userDTO;
        }
        // GET api/<UserApiController>/5
        [HttpGet("{id}")]
        //public async Task<IActionResult> getpeople()
        //{
        //    //IQueryable<User> userQry = _context.Users;
        //    //UserDTO[] user=await userQry
        //    //    .Select(u => new UserDTO
        //    //    {
        //    //        Name=u.Name,
        //    //        Email = u.Email,
        //    //        Gender = u.Gender,
        //    //        Birthday=u.Birthday, 
        //    //        Phone = u.Phone,
        //    //    })
        //    //    .ToArrayAsync();

        //    return Ok();
        //}
        // POST api/<UserApiController>
        [HttpPost]
        public async Task<bool> PostUser(PostUserVewModel register)
        {
            // 檢查用戶名與密碼是否為空
            if (string.IsNullOrEmpty(register.Name) || string.IsNullOrEmpty(register.Password))
            {
                return false;
            }
            // 對密碼進行加鹽

            try
            {
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
                    //Password = register.Password,
                    PasswordHash = hashedPassword,
                    Salt = salt,
                    CreateDate = DateTime.Now


                };

                // 添加用戶到資料庫

                _context.Users.Add(newUser);

                _context.SaveChanges();

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, $"{register.Name}"));
                claims.Add(new Claim("Email", register.Email));
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
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
        //[HttpGet("check-username")]
        //public async Task<ActionResult<bool>> CheckUsernameExists(string username)
        //{
        //    if (string.IsNullOrEmpty(username))
        //    {
        //        return BadRequest("使用者不能為空.");
        //    }

        //    bool usernameExists = await _context.Users.AnyAsync(user => user.Name == username);
        //    return Ok(new { Exists = usernameExists });
        //}



        // PUT api/<UserApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserViewModel UpdateUser)
        {

            if (id != UpdateUser.UserId)
            {
                return BadRequest("使用者不存在");
            }

            User user = await _context.Users.FindAsync(id);
            
            //判斷傳入的密碼是否更改

            if (UpdateUser.Password != null)
            {
                string hashedPassword = HashPassword(UpdateUser.Password, user.Salt);
                if (UpdateUser.PasswordHash == hashedPassword)
                {
                    return BadRequest("密碼不可重複");
                }
                else
                {
                    string salt = GenerateSalt();
                    user.Salt = salt;
                    user.PasswordHash = hashedPassword;
                }
            }

            // 修改其他個資
            user.Email = UpdateUser.Email;
            user.Birthday = UpdateUser.Birthday;
            user.Name = UpdateUser.Name;
            user.Phone = UpdateUser.Phone;
            user.Gender = UpdateUser.Gender;
            user.PasswordHash = UpdateUser.PasswordHash;
            user.Salt = UpdateUser.Salt;


            _context.Entry(UpdateUser).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Conflict();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationCode([FromBody] ForgotPasswordViewModel forget)
        {
            try
            {
                if (string.IsNullOrEmpty(forget.Email))
                    return BadRequest("郵件為必需的");

                string verificationCode = GenerateVerificationCode();
                string codeId = Guid.NewGuid().ToString();

                var verificationCodeData = new VerificationCodeData
                {
                    Code = verificationCode,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(10) // 設定驗證碼的有效期
                };

                _verificationCodes.TryAdd(codeId, verificationCodeData);

                await _emailSender.SendEmailAsync(forget.Email, "驗證碼", $"你的驗證碼: {verificationCodeData.Code}");

                return Ok(new { Message = "驗證碼成功寄送.", CodeId = codeId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"錯誤: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult VerifyCode([FromBody] VerifyCodeRequest request)
        {
           
            if (!_verificationCodes.TryGetValue(request.CodeId,out VerificationCodeData verificationCodeData ))
            {
                return BadRequest("錯誤的驗證碼或是驗證碼時效過期.");
            }

            if (verificationCodeData.ExpiryTime < DateTime.UtcNow)
            {
                _verificationCodes.TryRemove(request.CodeId, out _);
                return BadRequest("驗證碼過期.");
            }

            if (verificationCodeData.Code == request.Code)
            {
                _verificationCodes.TryRemove(request.CodeId, out _);
                return Ok("驗證碼驗證成功");
            }

            return BadRequest("錯誤驗證碼.");
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordViewModel request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user != null)
            {
                string password = request.NewPassword;
                string salt = GenerateSalt();

                string hashedPassword = HashPassword(password, salt);

                User userId = await _context.Users.FindAsync(id);
                user.PasswordHash = hashedPassword;
                user.Salt = salt;

                await _context.SaveChangesAsync();
                return Ok(new { Message = "密碼成功重設" });
            }
            else
            {
                return BadRequest(new { Message = "重設密碼" });
            }
        }

    }

 
   

    public class VerificationCodeData
    {
        public string Code { get; set; }
        public DateTime ExpiryTime { get; set; }
    }


}
    










