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
        public async Task<IActionResult> Login(string username, string password)
        {
           
                var userselect = _context.Users.Select(u => u.Email == username).SingleOrDefault();

                if (userselect == null)
                {
                    return View("Login");

                }
                string pw = Request.Form["password"].ToString();
                UserDTO userDTO = new UserDTO();
                if (userDTO.PasswordHash== HashPassword(pw, userDTO.Salt))
                {

                    var claims = new List<Claim>()//身份驗證訊息
                     {
                        new Claim(ClaimTypes.Name,$"{userDTO.Name}"),
                        new Claim("Email",userDTO.Email),
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
        [HttpPost]
        public IActionResult Register(UserDTO user)
        {
            // 檢查用戶名與用法是否為空
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Message = "帳號或密碼已被使用";
                return View();
            }
            string salt = GenerateSalt();

            // 對密碼進行加鹽
            string hashedPassword = HashPassword(user.Password, salt);

            // 創建用戶實體
            User newUser = new User
            {
                Name = user.Name,
                Gender = user.Gender,
                Email = user.Email,
                Birthday = user.Birthday,
                Password=user.Password,
                PasswordHash = hashedPassword,
                Salt = salt,
            };

            // 添加用戶到資料庫
            _context.Users.Add(newUser);
            _context.SaveChanges();

            ViewBag.Message = "會員成功註冊.";
            return View();
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
        public IActionResult SendEmail(string username, string password)
        {
            return View();
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

        // Custom method to validate the user
        private bool IsValidUser(string username, string password)
        {
            // Perform your custom validation logic here
            return (username == "example" && password == "password");
        }

        private void AuthenticateUser(string username)
        {
            // Perform your custom user authentication and session setup here
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, "custom");
            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync("custom", principal);
        }
        //[ValidateAntiForgeryToken]
        //public ActionResult SendMailToken(/*SendMailTokenIn inModel*/)
        //{
        //    SendMailTokenOut outModel = new SendMailTokenOut();

        //    if (string.IsNullOrEmpty(inModel.UserID))
        //    {
        //        outModel.ErrMsg = "請輸入帳號";
        //        return Json(outModel);
        //    }
        //    IConfigurationRoot Config=new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings").Build();
        //    string strConn = Config.GetConnectionString("TravelProject");
        //        using (SqlConnection conn = new SqlConnection(strConn))
        //         {

        //        conn.Open();


        //        string sql = "select * from Member where UserID = @UserID";
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandText = sql;
        //        cmd.Connection = conn;

        //        // 使用參數化填值
        //        cmd.Parameters.AddWithValue("@UserID", inModel.UserID);

        //        // 執行資料庫查詢動作
        //        SqlDataAdapter adpt = new SqlDataAdapter();
        //        adpt.SelectCommand = cmd;
        //        DataSet ds = new DataSet();
        //        adpt.Fill(ds);
        //        DataTable dt = ds.Tables[0];

        //        if (dt.Rows.Count > 0)
        //        {
        //            //取出會員信箱
        //            string UserEmail = dt.Rows[0]["UserEmail"].ToString();

        //            // 取得系統自定密鑰，在 Web.config 設定
        //            string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

        //            // 產生帳號+時間驗證碼
        //            string sVerify = inModel.UserID + "|" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        //            // 將驗證碼使用 3DES 加密
        //            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        //            MD5 md5 = new MD5CryptoServiceProvider();
        //            byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
        //            byte[] result = md5.ComputeHash(buf);
        //            string md5Key = BitConverter.ToString(result).Replace("-", "").ToLower().Substring(0, 24);
        //            DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
        //            DES.Mode = CipherMode.ECB;
        //            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
        //            byte[] Buffer = UTF8Encoding.UTF8.GetBytes(sVerify);
        //            sVerify = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length)); // 3DES 加密後驗證碼

        //            // 將加密後密碼使用網址編碼處理
        //            sVerify = HttpUtility.UrlEncode(sVerify);

        //            // 網站網址
        //            string webPath = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Content("~/");

        //            // 從信件連結回到重設密碼頁面
        //            string receivePage = "Member/ResetPwd";

        //            // 信件內容範本
        //            string mailContent = "請點擊以下連結，返回網站重新設定密碼，逾期 30 分鐘後，此連結將會失效。<br><br>";
        //            mailContent = mailContent + "<a href='" + webPath + receivePage + "?verify=" + sVerify + "'  target='_blank'>點此連結</a>";

        //            // 信件主題
        //            string mailSubject = "[測試] 重設密碼申請信";

        //            // Google 發信帳號密碼
        //            string GoogleMailUserID = ConfigurationManager.AppSettings["GoogleMailUserID"];
        //            string GoogleMailUserPwd = ConfigurationManager.AppSettings["GoogleMailUserPwd"];

        //            // 使用 Google Mail Server 發信
        //            string SmtpServer = "smtp.gmail.com";
        //            int SmtpPort = 587;
        //            MailMessage mms = new MailMessage();
        //            mms.From = new MailAddress(GoogleMailUserID);
        //            mms.Subject = mailSubject;
        //            mms.Body = mailContent;
        //            mms.IsBodyHtml = true;
        //            mms.SubjectEncoding = Encoding.UTF8;
        //            mms.To.Add(new MailAddress(UserEmail));
        //            using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
        //            {
        //                client.EnableSsl = true;
        //                client.Credentials = new NetworkCredential(GoogleMailUserID, GoogleMailUserPwd);//寄信帳密 
        //                client.Send(mms); //寄出信件
        //            }
        //            outModel.ResultMsg = "請於 30 分鐘內至你的信箱點擊連結重新設定密碼，逾期將無效";
        //        }
        //        else
        //        {
        //            outModel.ErrMsg = "查無此帳號";
        //        }
        //    }

        //    // 回傳 Json 給前端
        //    return Json(outModel);
        //}

    }
}


