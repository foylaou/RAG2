using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace RAG2.Controllers
{
    public class PdfController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Uploadpdf(IFormFile formData)
        {
            if (formData != null)
            {
                string guid = Guid.NewGuid().ToString();
                // 獲取檔案名（包含副檔名）
                // var fileName = Path.GetFileName(formData.FileName);
                // 獲取副檔名
                var fType = Path.GetExtension(formData.FileName.ToString());
                // 定義檔保存路徑
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", guid+fType);

                // 確保目錄存在
                Directory.CreateDirectory(Path.GetDirectoryName(savePath) ?? throw new InvalidOperationException());

                // 保存檔到伺服器
                await using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await formData.CopyToAsync(fileStream);
                }
                TempData["Message"] = "上傳成功";
                return RedirectToAction("Index"); // 復位向到某個動作
            }

            TempData["Message"] = "沒有收到檔";
            return RedirectToAction("Index");
        }
    }
}