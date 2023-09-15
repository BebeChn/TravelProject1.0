﻿using Microsoft.AspNetCore.Authentication.Cookies;
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
using Microsoft.Win32;
using Microsoft.AspNetCore.Authorization;

namespace TravelProject1._0.Controllers
{

    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;
       
        public UserController(ILogger<HomeController> logger, TravelProjectAzureContext context/*, EmailSender sender*/)

        {
            _logger = logger;
            _context = context;
            //_sender = sender;
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
            var userselect = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (userselect == null)
            {
                return View("Login");
            }

            string hashedPassword = HashPassword(password, userselect.Salt);

            if (userselect.PasswordHash == hashedPassword)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userselect.UserId.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, userselect.Name));
                claims.Add(new Claim(ClaimTypes.Email, userselect.Email));
                claims.Add(new Claim(ClaimTypes.Role, "user"));
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                }).Wait();

                return Redirect("/Home/Index");
            }
            else
            {
                ViewBag.LoginFailed = true;
                return View("Login");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }

        public IActionResult Register()
        {
            ViewData["Title"] = "商品瀏覽";
            return View();
        }

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
        private bool IsValidUser(string Email,string password)
        {
           
            UserDTO user = new UserDTO();
            return (Email=="user.Email" && password == "user.password");
        }
        [HttpGet]
        public IActionResult VerifyCode()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UserCenter()
        {
            return View();
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
}


