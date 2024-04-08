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
        public IActionResult Index(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                var data = new { Error = "不得為空值" };
                return Json(data); // 在ASP.NET Core中，直接使用Json方法
            }



            
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: user?.Salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            // 假设Session已经在Startup中配置好
            if (user.Password == hashedPassword)
            {
                Console.WriteLine("登入成功");
                var successData = new { message = "登入成功" };
                
                return Json(successData);
            }

            var loginerrorData = new { message = "帳號或密碼錯誤" };
            return Json(loginerrorData);
        }
    }
}