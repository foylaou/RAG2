using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using RAG2.Context;

namespace RAG2.Controllers
{
    public class LoginController : Controller
    {
        private readonly MyDbContext _db;

        public LoginController(MyDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormCollection post)
        {
            string username = post["username"];
            string password = post["password"];
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Msg = "不得為空值";
                return View();
            }
            
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                ViewBag.Msg = "帳號不存在";
                return View();
            }
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: user.Salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            // 假设Session已经在Startup中配置好
            if (user.Password == hashedPassword)
            {
                HttpContext.Session.SetString("login","login");
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                if (user.Username != null) HttpContext.Session.SetString("Username", user.Username.ToString());
                if (user.FirstName != null) HttpContext.Session.SetString("FirstName", user.FirstName.ToString());
                if (user.LastName != null) HttpContext.Session.SetString("LastName", user.LastName.ToString());
                return Redirect("/home");
            }
            Thread.Sleep(5000);
            ViewBag.Msg = "帳號或密碼錯誤";
            return View();
        }
        public ActionResult Logout(IFormCollection post)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");  // 重定向到首页
        }
    }
}