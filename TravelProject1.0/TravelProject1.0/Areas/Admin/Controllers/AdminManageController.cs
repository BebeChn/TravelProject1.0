using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelProject1._0.Models;

namespace TravelProject1._0.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("[area]/Manage/[action]")]
	public class AdminManageController : Controller
	{
        private readonly TravelProjectAzureContext _context;

        public AdminManageController(TravelProjectAzureContext context)
        {
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
        public IActionResult Login(string account,string password)
        {
            var admin = _context.Admins.FirstOrDefault(a=>a.Account==account&&a.Password==password);
            if (admin == null)
            {
                return View();
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, admin.Account));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Admin");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync("Admin", principal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
            }).Wait();
            TempData["SuccessMessage"] = "登入成功";
            return Redirect("/Admin/Manage/User");
        }
    }
}
