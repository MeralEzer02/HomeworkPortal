using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Text;

namespace HomeworkPortal.UI.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            try
            {
                var payload = token.Split('.')[1];
                payload = payload.Replace('-', '+').Replace('_', '/');
                payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
                var bytes = Convert.FromBase64String(payload);
                var json = Encoding.UTF8.GetString(bytes);
                var parsed = JsonSerializer.Deserialize<JsonElement>(json);

                if (parsed.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var roleProp))
                    ViewBag.Role = roleProp.GetString();
                else if (parsed.TryGetProperty("role", out roleProp))
                    ViewBag.Role = roleProp.GetString();
            }
            catch { }

            base.OnActionExecuting(context);
        }
    }
}