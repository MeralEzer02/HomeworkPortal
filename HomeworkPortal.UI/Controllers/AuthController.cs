using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.UI.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult SetSession(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Session.SetString("Token", token);
                return Ok();
            }
            return BadRequest("Token boş olamaz");
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
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }
    }
}