using Microsoft.AspNetCore.Mvc;
using HomeworkPortal.UI.Services;

namespace HomeworkPortal.UI.Controllers
{
    public class CoursesController : BaseController
    {
        public CoursesController(ITokenParserService tokenParser) : base(tokenParser)
        {
        }

        public IActionResult Index()
        {
            if (!_tokenParser.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
    }
}