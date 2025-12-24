using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TodoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new
            {
                message = "✅ Это публичный метод - доступен без авторизации",
                timestamp = DateTime.UtcNow,
                server = Environment.MachineName
            });
        }

        [HttpGet("auth-test")]
        [Authorize] // Требует только аутентификации
        public IActionResult AuthTest()
        {
            var identity = HttpContext.User.Identity;
            var claims = HttpContext.User.Claims.ToList();

            return Ok(new
            {
                message = "✅ Это защищенный метод - работает если ты аутентифицирован",
                isAuthenticated = identity?.IsAuthenticated,
                userName = identity?.Name,
                authenticationType = identity?.AuthenticationType,
                allClaims = claims.Select(c => new
                {
                    type = c.Type,
                    value = c.Value
                }).ToList(),
                roles = claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).ToList(),
                userId = claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            });
        }

        [HttpGet("admin-test")]
        [Authorize(Roles = "admin")] // У тебя в токене роль "admin" (маленькими)
        public IActionResult AdminTest()
        {
            return Ok(new
            {
                message = "✅ Это метод только для пользователей с ролью admin",
                user = User.Identity?.Name,
                userRoles = User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).ToList(),
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("user-test")]
        [Authorize(Roles = "user")] // Изменил с "User" на "user"
        public IActionResult UserTest()
        {
            return Ok(new
            {
                message = "✅ Это метод только для пользователей с ролью user",
                user = User.Identity?.Name,
                userRoles = User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).ToList(),
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("any-role-test")]
        [Authorize(Roles = "user,admin")] // Изменил регистр
        public IActionResult AnyRoleTest()
        {
            return Ok(new
            {
                message = "✅ Это метод для пользователей с ролью user ИЛИ admin",
                user = User.Identity?.Name,
                roles = User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).ToList(),
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("check-roles")]
        [Authorize]
        public IActionResult CheckRoles()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var hasAdminRole = roles.Any(r => r.Equals("admin", StringComparison.OrdinalIgnoreCase));
            var hasUserRole = roles.Any(r => r.Equals("user", StringComparison.OrdinalIgnoreCase));

            return Ok(new
            {
                message = "🔍 Проверка ролей пользователя",
                user = User.Identity?.Name,
                roles = roles,
                hasAdminRole = hasAdminRole,
                hasUserRole = hasUserRole,
                canAccessAdminTest = hasAdminRole,
                canAccessUserTest = hasUserRole,
                canAccessAnyRoleTest = hasAdminRole || hasUserRole
            });
        }

        [HttpGet("test-all")]
        public IActionResult TestAll()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            var roles = isAuthenticated
                ? User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList()
                : new List<string>();

            return Ok(new
            {
                isAuthenticated = isAuthenticated,
                userName = User.Identity?.Name,
                roles = roles,
                endpoints = new[]
                {
                    new { url = "/api/debug/public", method = "GET", access = "Все" },
                    new { url = "/api/debug/auth-test", method = "GET", access = "Только аутентифицированные" },
                    new { url = "/api/debug/admin-test", method = "GET", access = "Роль: admin" },
                    new { url = "/api/debug/user-test", method = "GET", access = "Роль: user" },
                    new { url = "/api/debug/any-role-test", method = "GET", access = "Роль: user ИЛИ admin" },
                    new { url = "/api/debug/check-roles", method = "GET", access = "Только аутентифицированные" },
                }
            });
        }

        [HttpGet("headers")]
        public IActionResult Headers()
        {
            var headers = Request.Headers
                .ToDictionary(h => h.Key, h => h.Value.ToString());

            return Ok(new
            {
                message = "📋 Заголовки текущего запроса",
                headers = headers,
                hasAuthHeader = headers.ContainsKey("Authorization"),
                authHeaderValue = headers.ContainsKey("Authorization")
                    ? (headers["Authorization"].Length > 50
                        ? headers["Authorization"].Substring(0, 50) + "..."
                        : headers["Authorization"])
                    : null
            });
        }
    }
}