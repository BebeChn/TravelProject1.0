using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;
using NuGet.Protocol;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Data.SqlClient;

namespace TravelProject1._0.Controllers
{

    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;
        private readonly EmailSender _sender;
        public UserController(ILogger<HomeController> logger, TravelProjectAzureContext context, EmailSender sender)

        {
            _logger = logger;
            _context = context;
            _sender = sender;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {

            var userselect = _context.Users.Select(u => u.Email == username).SingleOrDefault();

            if (userselect == null)
            {
                return View("Login");

            }
            string pw = Request.Form["password"].ToString();
            UserDTO userDTO = new UserDTO();
            if (userDTO.PasswordHash == HashPassword(pw, userDTO.Salt))
            {

                var claims = new List<Claim>()//身份驗證訊息
                     {
                        new Claim(ClaimTypes.Name,$"{userDTO.Name}"),
                        new Claim("Email",userDTO.Email),
                       };

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),//過期時間;30分鐘

                }).Wait();

                return Redirect("/Home/Index");
            }
            else
            {
                base.ViewBag.Msg = "用戶或密碼錯誤";
            }

            return await Task.FromResult<IActionResult>(View());
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task  <IActionResult> Register(UserDTO user)
        {
            // 檢查用戶名與用法是否為空
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Message = "帳號或密碼已被使用";
                return View();
            }
            string salt = GenerateSalt();

            // 對密碼進行加鹽
            string hashedPassword = HashPassword(user.Password, salt);

            // 創建用戶實體
            User newUser = new User
            {
                Name = user.Name,
                Gender = user.Gender,
                Email = user.Email,
                Birthday = user.Birthday,
                Password = user.Password,
                PasswordHash = hashedPassword,
                Salt = salt,
            };

            // 添加用戶到資料庫
            _context.Users.Add(newUser);
            _context.SaveChanges();

           //註冊視同登入
            List<Claim> claims = new List<Claim>();
            new Claim(ClaimTypes.Name, $"{user.Name}");
            new Claim("Email", user.Email);    
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

        public IActionResult ResetPassword()
        {
            return View();
        }
        public IActionResult EditProfile()
        {
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }

        // Custom method to validate the user
        private bool IsValidUser(string username, string password)
        {
            // Perform your custom validation logic here
            return (username == "example" && password == "password");
        }

        private void AuthenticateUser(string username)
        {
            // Perform your custom user authentication and session setup here
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, "custom");
            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync("custom", principal);
        }
        [HttpGet]
        public IActionResult SendVerificationCode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationCode(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {

                string verificationCode = GenerateVerificationCode();
                user.VerificationCode = verificationCode;
                _context.SaveChanges();


                await _sender.SendEmailAsync(email, "驗證碼", $"你的驗證碼: {verificationCode}");

                return RedirectToAction("VerifyCode");
            }
            else
            {
                ModelState.AddModelError("", "不存在的Email");
                return View();
            }
        }

        private string GenerateVerificationCode()
        {

            return new Random().Next(1000, 9999).ToString();
        }

        [HttpGet]
        public IActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyCode(string code)
        {

            var user = _context.Users.FirstOrDefault(u => u.VerificationCode == code);
            if (user != null)
            {
                // Code is valid, you can proceed with further actions
                // For example, mark the email as verified or allow password reset
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "不合法的驗證碼");

                return View();
            }
        }
    }


}


