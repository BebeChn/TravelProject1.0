using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.ViewModel;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AdminController : Controller
    {
        private readonly TravelProjectContext _db;

        public AdminController(TravelProjectContext db)
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

        //查詢
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
        //api/Admin/AdminPOST
        //關鍵字
        [HttpPost]
        public async Task<IEnumerable<AdminDTO>> AdminPOST(AdminDTO AdminDTO)
        {
            return _db.Users.Where(y=>
            y.UserId== AdminDTO.UserId||
            y.Name.Contains(AdminDTO.Name)||
            y.Gender.Contains( AdminDTO.Gender)||
            y.Address.Contains(AdminDTO.Address)||
            y.Email.Contains(AdminDTO.Email)
            ).Select(x => new AdminDTO
            {   
                UserId = x.UserId,
                Name = x.Name,
                Address = x.Address,
                Gender = x.Gender,
                Email = x.Email,
            });

             
        }





    }
}
