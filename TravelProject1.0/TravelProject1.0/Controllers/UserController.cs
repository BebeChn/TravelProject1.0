using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;


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
        //public IActionResult Register(User UserID)
        //{

        //    if (ModelState.IsValid)
        //    {

        //        return View();
        //    }
        //    return View();
        //    //var user = _context.User.Where(m => m.Id == User.UserID).FirstOrDefault();
        //    //if (user == null)
        //    //{
        //    //    _context.UserDTO.Add(user);
        //    //    _context.SaveChanges();
        //    //    return RedirectToAction("Login");
        //    //}
        //    //return View();
        //}
    }
}

