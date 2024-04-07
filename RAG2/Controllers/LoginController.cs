using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RAG2.Controllers
{
    public class LoginController : Controller
    {
        private readonly MariaDBContext _db;

        public LoginController(MariaDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                var data = new { Error = "不得為空值" };
                return Json(data); // 在ASP.NET Core中，直接使用Json方法
            }

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            
            // 假设Session已经在Startup中配置好
            if (user != null && user.Password == hashedPassword)
            {
                Console.WriteLine("登入成功");
                var successData = new { message = "登入成功" };
                return Json(successData);

                // HttpContext.Session.SetString("login", "true");
                // // HttpContext.Session.SetString("authority", user.Authority); // 您可能需要根据实际情况调整
                // return RedirectToAction("Index", "Home");
            }

            return View(); // 如果验证失败，返回登录页面
        }
    }
}