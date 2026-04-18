using System.Text.Json;
using System.Text;

namespace HomeworkPortal.UI.Services
{
    public class TokenParserService : ITokenParserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenParserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private JsonElement? GetParsedToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            if (string.IsNullOrEmpty(token)) return null;

            try
            {
                var payload = token.Split('.')[1];
                payload = payload.Replace('-', '+').Replace('_', '/');
                payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

                var bytes = Convert.FromBase64String(payload);
                var json = Encoding.UTF8.GetString(bytes);

                return JsonSerializer.Deserialize<JsonElement>(json);
            }
            catch
            {
                return null;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.Session.GetString("Token") != null;

        public string Role
        {
            get
            {
                var parsed = GetParsedToken();
                if (parsed == null) return "";

                if (parsed.Value.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var roleProp))
                    return roleProp.GetString() ?? "";
                if (parsed.Value.TryGetProperty("role", out roleProp))
                    return roleProp.GetString() ?? "";

                return "";
            }
        }

        public string Email
        {
            get
            {
                var parsed = GetParsedToken();
                if (parsed == null) return "";

                if (parsed.Value.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", out var emailProp))
                    return emailProp.GetString() ?? "";
                if (parsed.Value.TryGetProperty("email", out emailProp))
                    return emailProp.GetString() ?? "";

                return "";
            }
        }

        public string UserId
        {
            get
            {
                var parsed = GetParsedToken();
                if (parsed == null) return "";

                if (parsed.Value.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", out var idProp))
                    return idProp.GetString() ?? "";
                if (parsed.Value.TryGetProperty("sub", out idProp))
                    return idProp.GetString() ?? "";

                return "";
            }
        }
    }
}