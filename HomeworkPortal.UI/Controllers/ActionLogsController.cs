using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace HomeworkPortal.UI.Controllers
{
    public class ActionLogsController : Controller
    {
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("Token") ?? Request.Cookies["jwt_token"];

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            string role = "";

            try
            {
                var parts = token.Split('.');
                if (parts.Length >= 2)
                {
                    var payload = parts[1].Replace('-', '+').Replace('_', '/');

                    switch (payload.Length % 4)
                    {
                        case 2: payload += "=="; break;
                        case 3: payload += "="; break;
                    }

                    var jsonBytes = Convert.FromBase64String(payload);
                    var jsonString = Encoding.UTF8.GetString(jsonBytes);

                    using (var doc = JsonDocument.Parse(jsonString))
                    {
                        if (doc.RootElement.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var roleElement))
                        {
                            role = roleElement.GetString() ?? "";
                        }
                        else if (doc.RootElement.TryGetProperty("role", out roleElement))
                        {
                            role = roleElement.GetString() ?? "";
                        }
                    }
                }
            }
            catch
            {
            }

            if (role != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Role = role;
            return View();
        }
    }
}