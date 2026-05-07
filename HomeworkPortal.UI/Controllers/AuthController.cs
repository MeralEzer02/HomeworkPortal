using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 

namespace HomeworkPortal.UI.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token)) return BadRequest("Token boş olamaz");

            HttpContext.Session.SetString("Token", token);
            return Ok();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}