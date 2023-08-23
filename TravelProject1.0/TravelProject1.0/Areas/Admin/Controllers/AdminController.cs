using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AdminController : Controller
    {
        private readonly TravelProjectAzureContext _db;

        public AdminController(TravelProjectAzureContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Admin_partial()
        {
            return View();
        }


        [HttpGet]
        public async Task<IEnumerable<AdminDTO>> AdminGET()
        {
            return _db.Users.Select(x => new AdminDTO
            {

                Address = x.Address,
                Gender = x.Gender,
                Name = x.Name,
                UserId = x.UserId,
                Email = x.Email,
               
                
            }); 

        }

    [HttpPost]
        public async Task<IEnumerable<AdminDTO>> AdminSearch(AdminDTO AdminDTO)
        {
            return _db.Users.Where(y =>
            y.UserId == AdminDTO.UserId ||
            y.Name.Contains(AdminDTO.Name) ||
            y.Gender.Contains(AdminDTO.Gender) ||
            y.Address.Contains(AdminDTO.Address) ||
            y.Email.Contains(AdminDTO.Email)
            || y.CreateDate==(AdminDTO.CreateDate)
            ).Select(x => new AdminDTO
            {
                UserId = x.UserId,
                Name = x.Name,
                Gender = x.Gender,
                Address = x.Address,
                Email = x.Email,
                CreateDate = x.CreateDate,
            });
        }

        [HttpGet("{id}")]
        public async Task<AdminDTO> AdminGETID(int id)
        {
            if (_db.Users == null)
            {
                return null;
            }
            var User = await _db.Users.FindAsync(id);
            AdminDTO UserDTO = new AdminDTO
            {
                UserId = User.UserId,
                Name = User.Name,
                Gender = User.Gender,
                Address = User.Address,
                Email = User.Email,
            };
            return UserDTO;
        }


        [HttpPut("{id}")]
        public async Task<string> AdminPUT(int id, AdminDTO AdminDTO)
        {
            if (id != AdminDTO.UserId)
            {
                return "修改失敗";
            }
            var User = await _db.Users.FindAsync(id);
            User.Name = AdminDTO.Name;
            User.Gender = AdminDTO.Gender;
            User.Address = AdminDTO.Address;
            User.Email = AdminDTO.Email;
            User.CreateDate = AdminDTO.CreateDate;
            _db.Entry(User).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }

            return "成功";
        }


        

        [HttpDelete("{id}")]
        public async Task<string> AdminDELETE(int id)
        {
            if (_db.Users == null)
            {
                return "刪除失敗";
            }
            var Users = await _db.Users.FindAsync(id);
            if (Users == null)
            {
                return "刪除失敗";
            }
            try
            {
                _db.Users.Remove(Users);
                await _db.SaveChangesAsync();
            }
            catch
            {
                return "刪除關聯失敗";
            }
            return "刪除成功";
        }




        //日期低高
        public async Task<IEnumerable<AdminDTO>> Datelower()
        {
            return _db.Users.OrderBy(y => y.CreateDate)
                .Select(z => new AdminDTO
                {
                    CreateDate = z.CreateDate,

                });
        }

        //日期高低
        public async Task<IEnumerable<AdminDTO>> Datehigh()
        {
            return _db.Users.OrderByDescending(y => y.CreateDate)
                .Select(z => new AdminDTO
                {
                    CreateDate = z.CreateDate,

                
                });
        }
    }
}
