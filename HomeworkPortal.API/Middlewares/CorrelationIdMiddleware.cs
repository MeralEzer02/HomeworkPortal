using Serilog.Context;

namespace HomeworkPortal.API.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault() ?? Guid.NewGuid().ToString();

            context.Response.Headers.Append(CorrelationIdHeader, correlationId);

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}