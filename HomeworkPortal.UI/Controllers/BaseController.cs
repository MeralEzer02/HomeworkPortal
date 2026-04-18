using Microsoft.AspNetCore.Mvc;
using HomeworkPortal.UI.Services;

namespace HomeworkPortal.UI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ITokenParserService _tokenParser;

        public BaseController(ITokenParserService tokenParser)
        {
            _tokenParser = tokenParser;
        }

        protected string UserId => _tokenParser.UserId;
        protected string Role => _tokenParser.Role;
        protected string Email => _tokenParser.Email;

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.Role = Role;
            ViewBag.UserId = UserId;
            ViewBag.Email = Email;
        }
    }
}