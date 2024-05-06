using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RAG2.Controllers;

public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // 获取控制器名称
        string sController = filterContext.RouteData.Values["controller"].ToString().ToUpper();
        // 定义无需登录检查的控制器
        string sBypassController = "AUDITEDIT";

        // 检查当前控制器是否需要登录检查
        if (sController != sBypassController)
        {
            // 检查 Session 中的 "login" 是否存在或为空
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("login")))
            {
                // 如果未登录，重定向到登录页
                filterContext.Result = Redirect("~/Login");
            }
        }

        // 调用基类的 OnActionExecuting 来继续执行
        base.OnActionExecuting(filterContext);
    }
}