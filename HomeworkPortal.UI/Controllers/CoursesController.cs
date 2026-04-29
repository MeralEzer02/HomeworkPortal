using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.UI.Controllers
{
    public class CoursesController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}