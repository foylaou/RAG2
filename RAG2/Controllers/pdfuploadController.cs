using Microsoft.AspNetCore.Mvc;

namespace RAG2.Controllers;

public class pdfuploadController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            // 获取文件名（包含扩展名）
            var fileName = Path.GetFileName(file.FileName);

            // 定义文件保存路径
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

            // 确保目录存在
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            // 保存文件到服务器
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var data = new { Success = "上傳成功" };
            return Json(data); 

        }

        var errdata = new { error = "沒有收到文件" };
        return Json(errdata); 

    }
}