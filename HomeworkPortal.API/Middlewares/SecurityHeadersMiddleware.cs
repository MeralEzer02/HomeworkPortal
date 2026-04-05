namespace HomeworkPortal.API.Middlewares
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. XSS (Cross-Site Scripting) Koruması
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

            // 2. Clickjacking (Tıklama Sahtekarlığı / iframe içine alma) Koruması
            context.Response.Headers["X-Frame-Options"] = "DENY";

            // 3. MIME-Sniffing (Dosya türü taklidi) Engelleme
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";

            // 4. Referrer Policy (Hangi siteden gelindiği bilgisini gizleme)
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            // 5. Content Security Policy (Temel seviye)
            context.Response.Headers["Content-Security-Policy"] = "default-src 'self'";

            await _next(context);
        }
    }
}