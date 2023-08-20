using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace TravelProject1._0.Controllers
{

    public class UserController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectContext _context;
        public UserController(ILogger<HomeController> logger, TravelProjectContext context)

        {
            _logger = logger;
            _context = context;
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
        public async Task<IActionResult> Login(UserDTO user)
        {
            var userselect = _context.Users.Where(u => (u.Email == user.Email && u.Password == user.Password)).SingleOrDefault();

            if (userselect != null)
            {
                var claims = new List<Claim>()//身份驗證訊息
                    {
                        new Claim(ClaimTypes.Name,$"{user.Name}"),
                        new Claim("Email",user.Email),

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
       public IActionResult Register(string username, string password)
        {
            // 檢查用戶名與用法是否為空
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "Username and password are required.";
                return View();
            }
            string salt = GenerateSalt();

                // 對密碼進行加鹽
            string hashedPassword = HashPassword(password, salt);

                 // 創建用戶實體
                 User newUser = new User
                 {
                 Name = username,
                 PasswordHash = hashedPassword,
                 Salt = salt
                 };

            // 添加用戶到資料庫
                     _context.Users.Add(newUser);
                     _context.SaveChanges();

                   ViewBag.Message = "User registered successfully.";
                    return View();
        }   
        // 生成隨機鹽
        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            
            return Convert.ToBase64String(saltBytes);
        }

        // 使用SHA-256哈希密碼並加鹽
        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // 将密碼轉換成二進位
                string passwordWithSalt = password + salt;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(passwordWithSalt);
                // 計算密碼哈希
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                // 將密碼哈希轉換為Base64编馬的字符串
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}

