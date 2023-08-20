using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;

namespace TravelProject1._0.Controllers
{

    public class UserController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        public UserController(ILogger<HomeController> logger)

        {
            _logger = logger;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        //public IActionResult Login(string UserID, string password)
        //{
        //    //var number = _db.User.Where(u=>u.Id==UserID && u.PasswordHash==password).FirstOrDefault();
        //    //if (number == null)
        //    //{
        //    //    ViewBag.Message = "帳號或密碼錯誤,登入失敗";
        //    //    return View();
        //    //}
        //    //else 
        //    //{
        //    //    return RedirectToAction("Index");
        //    //}
        //}
        //public IActionResult Register()
        //{
        //    return View();
        //}
        //public IActionResult Register(User UserID )
        //{

        //    if (ModelState.IsValid)
        //    {

        //        return View();
        //    }
        //   var user= _db.User.Where(m=>m.Id==User.UserID).FirstOrDefault();
        //    if (user == null) 
        //    {
        //        _db.User.Add(user);
        //        _db.SaveChanges();
        //        return RedirectToAction("Login");
        //    }
        //    return View();
        //}
    }
}
