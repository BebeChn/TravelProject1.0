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
        //private readonly EmailSender _sender;
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

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
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

      
        private bool IsValidUser(string Email,string password)
        {
           
            UserDTO user = new UserDTO();
            return (Email=="user.Email" && password == "user.password");
        }



        //[HttpPost]
        //public async Task<IActionResult> SendVerificationCode(string email)
        //{
        //    var user = _context.Users.FirstOrDefault(u => u.Email == email);
        //    if (user != null)
        //    {

        //        string verificationCode = GenerateVerificationCode();
        //        user.VerificationCode = verificationCode;
        //        _context.SaveChanges();


        //        await _sender.SendEmailAsync(email, "驗證碼", $"你的驗證碼: {verificationCode}");

        //        return RedirectToAction("VerifyCode");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "不存在的Email");
        //        return View();
        //    }
        //}



        [HttpGet]
        public IActionResult VerifyCode()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult VerifyCode(string code)
        //{

        //    var user = _context.Users.FirstOrDefault(u => u.VerificationCode == code);
        //    if (user != null)
        //    {
        //        // Code is valid, you can proceed with further actions
        //        // For example, mark the email as verified or allow password reset
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "不合法的驗證碼");

        //        return View();
        //    }
        //}
    }




}


