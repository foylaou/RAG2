using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using RAG2.Entities;

namespace RAG2.Controllers;

public class SignUpController : Controller
{

    private readonly MariaDBContext _db;

    public SignUpController(MariaDBContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(string username, string password, string email)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
        {
            var data = new { Error = "不得為空值" };
            return Json(data);
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

        if (_db.Users.Any(u => u.Username == username))
        {
            var data = new { Error = "使用者名稱重複" };
            return Json(data);
        }
        
        if (_db.Users.Any(u => u.Email == email))
        {
            var data = new { Error = "信箱重複註冊" };
            return Json(data);
        }

        var user = new User()
        {
            Username = username,
            Password = hashedPassword,
            Email = email,
            
        };
        System.Console.WriteLine(user);
        _db.Users.Add(user);
        _db.SaveChanges();

        var successData = new { message = "註冊成功" };
        return Json(successData);
        
    }
}