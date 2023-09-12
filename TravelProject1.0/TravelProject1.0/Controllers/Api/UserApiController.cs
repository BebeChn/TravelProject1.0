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
using Azure.Core;
using NuGet.Versioning;
using NuGet.Packaging.Rules;
using TravelProject1._0.Models.ProductDTO;
using NuGet.Protocol;
using AspNetCoreHero.ToastNotification.Helpers;
using Microsoft.AspNetCore.SignalR;
using TravelProject1._0.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/UserApi/[Action]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;
        private readonly ConcurrentDictionary<string, VerificationCode> _verificationCodes = new ConcurrentDictionary<string, VerificationCode>();
        private readonly IEmailSender _emailSender;
        private readonly IUserIdentityService _userIdentityService;
        public UserApiController(ILogger<HomeController> logger, TravelProjectAzureContext context, IEmailSender emailSender, IUserIdentityService userIdentityService)

        {
            _logger = logger;
            _context = context;
            _verificationCodes = new ConcurrentDictionary<string, VerificationCode>();
            _emailSender = emailSender;
            _userIdentityService = userIdentityService;
        }

        [HttpGet]
        public async Task<UpdateUserDTO> GetUser()
        {
            var userId = _userIdentityService.GetUserId();
            if (_context.Users == null)
            {
                return null;
            }
            var users = await _context.Users.FindAsync(userId);

            if (users == null)
            {
                return null;
            }
            UpdateUserDTO usersDTO = new UpdateUserDTO
            {
                Name = users.Name,
                Email = users.Email,
                Birthday = users.Birthday.Value.ToString("yyyy-MM-dd"),
                Gender = users.Gender,
                Phone = users.Phone,
            };
            return usersDTO;
        }

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
                claims.Add(new Claim(ClaimTypes.NameIdentifier, register.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, $"{register.Name}"));
                claims.Add(new Claim("Email", register.Email));
                claims.Add(new Claim(ClaimTypes.Role, "user"));
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
                // 計算哈希
                byte[] hashBytes = SHA256.ComputeHash(passwordBytes);
                // 將哈希轉換為Base64編碼的字串
                return Convert.ToBase64String(hashBytes);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel UpdateUser)
        {

            int id = _userIdentityService.GetUserId();

            User user = await _context.Users.FindAsync(id);

            // 修改其他個資
            user.Email = UpdateUser.Email;
            user.Birthday = UpdateUser.Birthday;
            user.Name = UpdateUser.Name;
            user.Phone = UpdateUser.Phone;
            user.Gender = UpdateUser.Gender;

            _context.Entry(user).State = EntityState.Modified;
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
        public async Task<IActionResult> SendVerificationCode(int id, [FromBody] ForgotPasswordViewModel forget)
        {
            try
            {
                if (string.IsNullOrEmpty(forget.Email))
                    return BadRequest("郵件為必需的");

                string verificationCode = GenerateVerificationCode();
                string codeId = Guid.NewGuid().ToString();

                var verificationCodeData = new VerificationCode
                {

                    Code = verificationCode,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(10) // 設定驗證碼的有效期
                };
                VerificationCode vc = await _context.VerificationCodes.FindAsync(id);
                _context.VerificationCodes.Add(verificationCodeData);

                _context.SaveChanges();

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
        public async Task<IActionResult> VerifyCode([FromBody] VerificationCodeViewModel request)
        {
            try
            {
                var verificationCodeData = await _context.VerificationCodes
             .FirstOrDefaultAsync(v => v.Code == request.Code);

                if (verificationCodeData == null)
                {
                    return BadRequest("錯誤的驗證碼或是驗證碼時效過期.");
                }
                //if (verificationCodeData.ExpiryTime < DateTime.UtcNow)
                //{
                //    _verificationCodes.TryRemove("request.CodeId", out _);
                //    return BadRequest("驗證碼過期.");
                //}
                _context.VerificationCodes.Remove(verificationCodeData);
                await _context.SaveChangesAsync();
                return Ok("驗證碼驗證成功");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"錯誤: {ex.Message}");
            }
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        [HttpPost]
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

        [HttpGet]
        public IEnumerable<OrderInfo> OrderDetails()
        {
            var userId = _userIdentityService.GetUserId();
            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Plan).Where(o => o.UserId == userId)
                .Select(o => new OrderInfo
                {
                    OrderDate = o.OrderDate,
                    OrderId = o.OrderId,
                    Detail = o.OrderDetails.Select(z => new OrderDetailDto
                    {
                        PlanId = z.PlanId,
                        Quantity = z.Quantity,
                        UnitPrice = z.UnitPrice,
                        Odimg = z.Odimg,
                        Odname = z.Odname,
                        ProductId = z.Plan.ProductId
                    })
                });
        }
    }
}