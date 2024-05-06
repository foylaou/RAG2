using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using RAG2.Context;
using RAG2.Entities;

namespace RAG2.Controllers;

public class SignUpController : Controller
{

    private readonly MyDbContext _db;

    public SignUpController(MyDbContext db)
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
        string email = post["email"];
        //資料輸入處理邏輯判斷空值或空白
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
        {
            ViewBag.Msg = "不得為空值";
            return View();
        }
        //產生SALT
        byte[] salt = new byte[128 / 8];
        //透過隨機方式產生SALT
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
        //邏輯判斷帳號名稱是否重複
        if (_db.Users.Any(u => u.Username == username))
        {
            ViewBag.Msg = "使用者名稱重複";
            return View();
        }
        //邏輯判斷帳信箱是否重複
        if (_db.Users.Any(u => u.Email == email))
        {
            ViewBag.Msg = "信箱重複註冊";
            return View();
        }
        //將註冊資料寫近資料庫
        var user = new User()
        {
            Username = username, //帳號
            Password = hashedPassword, //密碼
            Email = email, //電子郵件
            Salt = salt //密碼加密SALT
            
        };
        System.Console.WriteLine(user);
        _db.Users.Add(user);
        _db.SaveChanges();

        var successData = new { message = "註冊成功" };
        return Json(successData);
        
    }
}