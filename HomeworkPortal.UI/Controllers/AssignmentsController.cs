using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.UI.Controllers
{
    public class AssignmentsController : BaseController
    {
        public IActionResult Index(int? courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }
    }
}