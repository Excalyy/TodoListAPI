using System.Net;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;  
using Microsoft.AspNetCore.Http;

namespace TodoListAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Произошла необработанная ошибка");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Внутренняя ошибка сервера";

            // Используем switch по типу с pattern matching
            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized; // 401
                    message = "Требуется авторизация или недостаточно прав";
                    break;

                case SecurityTokenExpiredException:
                case SecurityTokenInvalidSignatureException:
                case SecurityTokenException:
                    statusCode = HttpStatusCode.Unauthorized; // 401
                    message = "Неверный или просроченный токен";
                    break;

                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    message = argEx.Message;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError; // 500
                    message = "Внутренняя ошибка сервера";
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                title = statusCode.ToString(),
                status = (int)statusCode,
                detail = message,
                instance = context.Request.Path.Value
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}