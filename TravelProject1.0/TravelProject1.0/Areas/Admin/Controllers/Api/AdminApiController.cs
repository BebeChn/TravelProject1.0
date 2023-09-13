﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelProject1._0.Areas.Admin.Models.DTO;
using TravelProject1._0.Areas.Admin.Models.ViewModel;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Areas.Admin.Controllers.Api
{
    [Area("Admin")]
    [Route("api/AdminApi/[action]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _context;
        public AdminApiController(TravelProjectAzureContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<AdminGetUserViewModel> AdminGetUser()
        {
            return _context.Users.Select(x => new AdminGetUserViewModel

			{
                Id = x.UserId,
                Email = x.Email,
                Gender = x.Gender,
                Name = x.Name,
                Phone = x.Phone,
                Birthday=x.Birthday.Value.ToString("yyyy-MM-dd"),
            });

        }


        [HttpPost]
        public bool AdminManageUser(AdmminManageUserDTO amuDTO)
        {
            var salt = GenerateSalt();

            var passwordhash = HashPassword(amuDTO.Password, salt);

            User insertuser = new User
            {
                Name = amuDTO.Name,
                Email = amuDTO.Email,
                PasswordHash = passwordhash,
                Gender = amuDTO.Gender,
                Phone = amuDTO.Phone,
                Birthday= amuDTO.Birthday,
            };
            try
            {
                _context.Users.AddAsync(insertuser);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, amuDTO.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, $"{amuDTO.Name}"));
            claims.Add(new Claim("Email", amuDTO.Email));
            claims.Add(new Claim(ClaimTypes.Role, "user"));
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            return true;
        }
        private string GenerateSalt()
        {
            Byte[] bytes = new Byte[16];
            using (var ran = RandomNumberGenerator.Create())
            {
                ran.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }
        private string HashPassword(string paswword, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                string paswwordsalt = paswword + salt;
                byte[] passwordhash = Encoding.UTF8.GetBytes(paswwordsalt);
                byte[] psaswordhash = sha256.ComputeHash(passwordhash);
                return Convert.ToBase64String(psaswordhash);
            }
        }
        [HttpPut]
        public async Task<string> AdminPutUser(int id, AdminPutUserDTO apuDTO) {

            var admin = await _context.Users.FindAsync(id);
            admin.Name = apuDTO.Name;
            admin.Gender = apuDTO.Gender;
            admin.Email = apuDTO.Email;
            admin.Birthday = apuDTO.Birthday;
            admin.Phone = apuDTO.Phone;

            _context.Entry(User).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return "成功";
        }
        [HttpDelete]
        public async Task<string> AdminDeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return "刪除失敗";
            }
            var Users = await _context.Users.FindAsync(id);
            if (Users == null)
            {
                return "刪除失敗";
            }
            try
            {
                _context.Users.Remove(Users);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return "刪除關聯失敗";
            }
            return "刪除成功";
        }

    }
}