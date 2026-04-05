using System.Security.Claims;

namespace HomeworkPortal.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Token içindeki NameIdentifier (Id) bilgisini döner
        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        // Token içindeki Name (UserName) bilgisini döner
        public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        // Kullanıcı giriş yapmış mı?
        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}