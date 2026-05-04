using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.UI.Controllers
{
    public class ActionLogsController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.Role = "Admin";
            return View();
        }
    }
}