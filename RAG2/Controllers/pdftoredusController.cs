using Microsoft.AspNetCore.Mvc;

namespace RAG2.Controllers;

public class pdftoredusController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
}