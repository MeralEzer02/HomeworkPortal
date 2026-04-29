using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.UI.Controllers
{
    public class UsersController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}