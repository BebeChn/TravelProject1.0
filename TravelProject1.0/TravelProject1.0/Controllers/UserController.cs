using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers;

public class UserController : Controller
{
    private readonly TravelProjectAzureContext _context;
    private readonly ILogger<HomeController> _logger;

    public UserController(ILogger<HomeController> logger, TravelProjectAzureContext context)

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
    public async Task<IActionResult> Login(string email, string password)
    {
        ViewData["LoginFailed"] = true;
        var user = await _context.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();

        if (user == null) return View("Login");

        var hashedPassword = HashPassword(password, user.Salt);

        if (user.PasswordHash == hashedPassword)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Role, "user"));
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                }).Wait();
            TempData["SuccessMessage"] = "登入成功";
            return Redirect("/Home/Index");
        }

        ViewBag.LoginFailed = true;
        return View("Login");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        ViewData["Title"] = "商品瀏覽";
        return View();
    }

    private string GenerateSalt()
    {
        var saltBytes = new byte[16];
        using (var ran = RandomNumberGenerator.Create())
        {
            ran.GetBytes(saltBytes);
        }

        return Convert.ToBase64String(saltBytes);
    }

    // 使用SHA-256哈希密碼並加鹽
    private string HashPassword(string password, string salt)
    {
        using (var SHA256 = System.Security.Cryptography.SHA256.Create())
        {
            // 將密碼轉換成二進位
            var passwordWithSalt = password + salt;
            var passwordBytes = Encoding.UTF8.GetBytes(passwordWithSalt);
            // 計算密碼哈希
            var hashBytes = SHA256.ComputeHash(passwordBytes);
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

    [Authorize]
    public IActionResult UserOrderDetails()
    {
        return View();
    }

    [Authorize]
    public IActionResult UpdateUser(int id)
    {
        return View();
    }

    private bool IsValidUser(string Email, string password)
    {
        var user = new UserDto();
        return Email == "user.Email" && password == "user.password";
    }

  

  

    public IActionResult UserCollect()
    {
        return View();
    }

    public IActionResult UserPoint()
    {
        return View();
    }

    public IActionResult UserOrder()
    {
        return View();
    }
}