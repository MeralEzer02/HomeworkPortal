using Microsoft.AspNetCore.Mvc;
using HomeworkPortal.UI.Services;

namespace HomeworkPortal.UI.Controllers
{
    public class AssignmentsController : BaseController
    {
        public AssignmentsController(ITokenParserService tokenParser) : base(tokenParser)
        {
        }

        // URL örneği: /Assignments?courseId=5 (Kursun içinden gelirsek)
        // URL örneği: /Assignments (Öğrenci sol menüden "Ödevlerim"e tıklarsa)
        public IActionResult Index(int? courseId)
        {
            if (!_tokenParser.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.CourseId = courseId; // Sayfaya (View) taşıyoruz
            return View();
        }
    }
}