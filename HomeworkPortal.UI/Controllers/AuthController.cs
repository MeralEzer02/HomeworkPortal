using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.UI.Controllers
{
    public class AuthController : BaseController
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
            var token = Request.Cookies["jwt_token"];
            if (!string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}