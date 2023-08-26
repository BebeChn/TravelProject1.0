﻿using Microsoft.AspNetCore.Authentication.Cookies;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/UserApi/[Action]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TravelProjectAzureContext _context;

        //private readonly EmailSender _emailSender;
        public UserApiController(ILogger<HomeController> logger, TravelProjectAzureContext context/*, EmailSender emailSender*/)

        {
            _logger = logger;
            _context = context;
            //_emailSender = emailSender;

        }
        // GET: api/<UserApiController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<UserApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> getpeople()
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
                    CreateDate = DateTime.Now,
                    Address = register.Address

                };

                // 添加用戶到資料庫

                _context.Users.Add(newUser);

                _context.SaveChanges();
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, $"{register.Name}"));
                claims.Add(new Claim("Email", register.Email));
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
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
        [HttpGet("check-username")]
        public async Task<ActionResult<bool>> CheckUsernameExists(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be empty.");
            }

            bool usernameExists = await _context.Users.AnyAsync(user => user.Name == username);
            return Ok(new { Exists = usernameExists });
        }



        // PUT api/<UserApiController>/5
        //[HttpPut("{id}")]
        //    public async Task<IActionResult>UpdateUser(int id,UpdateUserViewModel UpdateUser)
        //    {
        //        if (id != UpdateUser.UserId)
        //        {
        //            return BadRequest("使用者不存在");
        //        }

        //        User user = await _context.Users.FindAsync(id);


        //        if (user != null)
        //        {
        //            string hashedPassword = HashPassword(UpdateUser.Password,user.Salt);

        //            if (hashedPassword == UpdateUser.OldPassword)
        //                return BadRequest("密碼不可重複");
        //        }

        //        //判斷傳入的密碼是否更改

        //        if  (UpdateUser.Password != null)
        //        {
        //            string salt = GenerateSalt();
        //            string hashedPassword = HashPassword(UpdateUser.Password, salt);
        //            user.Password = UpdateUser.Password;
        //            user.Salt = salt;
        //            user.PasswordHash = hashedPassword;
        //        }

        //        // 修改其他個資
        //        user.Email = UpdateUser.Email;
        //        user.Address= UpdateUser.Address;
        //        user.Birthday = UpdateUser.Birthday;
        //        user.Name = UpdateUser.Name;
        //        user.Phone = UpdateUser.Phone;
        //        user.Gender = UpdateUser.Gender;

        //          _context.Entry(UpdateUser).State = EntityState.Modified;
        //          try
        //          {
        //           await _context.SaveChangesAsync();
        //          }
        //          catch
        //          {
        //            return Conflict();
        //          }
        //           return Ok();
        //    }
        //    private string GenerateResetToken()
        //    {
        //        var rng = RandomNumberGenerator.Create();
        //        var bytes = new byte[20];
        //        rng.GetBytes(bytes);
        //        return Convert.ToBase64String(bytes);
        //    }

        //    private string VerificationCode()
        //    {
        //        Random rng =new Random(5);
        //        var vertficationcode = rng.ToString();
        //        return vertficationcode;
        //    }
        //    [HttpPost("forgot")]
        //    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel request)
        //    {
        //        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

        //        if (user != null)
        //        {
        //            var resetToken = GenerateResetToken();
        //            user.ResetToken = resetToken;
        //            string verificationCode = GenerateResetToken();
        //            user.VerificationCode = VerificationCode();
        //            try
        //            { 

        //            await _context.SaveChangesAsync();
        //            }
        //            catch 
        //            {
        //                return BadRequest("資料庫更新失敗");
        //            }
        //            await _emailSender.SendEmailAsync(request.Email, "驗證碼", $"你的驗證碼: {VerificationCode}");
        //            return RedirectToPage("./ForgotPassword");
        //        }
        //        else
        //        {
        //           return NotFound(new { Message = "郵件無效" });
        //        }
        //    }

        //    [HttpPost("reset")]
        //    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel request)
        //    {
        //        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.ResetToken == request.ResetToken);
        //        if (user != null)
        //        {
        //            user.Password = request.NewPassword;
        //            user.ResetToken = null;
        //            await _context.SaveChangesAsync();
        //            return Ok(new { Message = "密碼成功重設" });
        //        }
        //        else
        //        {
        //            return BadRequest(new { Message = "重設密碼" });
        //        }
        //    }


    }
       
 }

    

    


