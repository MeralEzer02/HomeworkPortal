using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace HomeworkPortal.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            context.Response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var title = "Sunucu Hatası";
            var details = exception.Message;

            var traceId = context.TraceIdentifier;

            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    title = "Yetkisiz İşlem";
                    break;
                case DbUpdateConcurrencyException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    title = "Eşzamanlılık Çatışması (Concurrency)";
                    break;
                case DbUpdateException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    title = "Veritabanı Kural İhlali";
                    break;
                default:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    title = "İş Kuralı İhlali";
                    break;
            }

            logger.LogError(exception, "🚨 SİSTEM HATASI! TraceId: {TraceId}, Path: {Path}, Mesaj: {Message}", traceId, context.Request.Path, exception.Message);

            context.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = details,
                Instance = context.Request.Path
            };

            problemDetails.Extensions.Add("traceId", traceId);

            var result = JsonSerializer.Serialize(problemDetails);
            return context.Response.WriteAsync(result);
        }
    }
}