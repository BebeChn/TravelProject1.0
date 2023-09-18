using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models;
using TravelProject1._0.Models.ViewModel;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity.UI.Services;
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
			catch (Exception)
			{
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
		public async Task<bool> UpdateUser(UpdateUserViewModel UpdateUser)
		{
			try
			{
				int id = _userIdentityService.GetUserId();
				User user = await _context.Users.FindAsync(id);

				// 修改其他個資
				user.Email = UpdateUser.Email;
				user.Birthday = UpdateUser.Birthday;
				user.Name = UpdateUser.Name;
				user.Phone = UpdateUser.Phone;
				user.Gender = UpdateUser.Gender;
				await _context.SaveChangesAsync();
			}
			catch
			{
				return false;
			}
			return true;
		}

		[HttpPost]
		public async Task<bool> SendVerificationCode(int id, [FromBody] ForgotPasswordViewModel forget)
		{
			try
			{
				if (string.IsNullOrEmpty(forget.Email)) return false;

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

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		[HttpPost]
		public async Task<bool> VerifyCode([FromBody] VerificationCodeViewModel request)
		{
			try
			{
				var verificationCodeData = await _context.VerificationCodes
			 .FirstOrDefaultAsync(v => v.Code == request.Code);

				if (verificationCodeData == null)
				{
					return false;
				}
				_context.VerificationCodes.Remove(verificationCodeData);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private string GenerateVerificationCode()
		{
			Random random = new Random();
			return random.Next(1000, 9999).ToString();
		}

		[HttpPut]
		public async Task<bool> ResetPassword([FromBody] ResetPasswordViewModel request)
		{
			try
			{
				var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
				if (user != null) return false;
				string password = request.NewPassword;
				string salt = GenerateSalt();
				string hashedPassword = HashPassword(password, salt);
				user.PasswordHash = hashedPassword;
				user.Salt = salt;
				_context.Entry(user).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}