using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.UI.Controllers
{
    public class SubmissionsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}