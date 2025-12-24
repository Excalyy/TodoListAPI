using Microsoft.AspNetCore.Builder;

namespace TodoListAPI.Middleware  // Должен совпадать с namespace вашего ExceptionMiddleware
{
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Регистрирует глобальный обработчик исключений
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}