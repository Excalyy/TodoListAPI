using Microsoft.AspNetCore.Mvc;
using TodoListAPI.Services;
using TodoListAPI.Models.DTO;

namespace TodoListAPI.Controller  
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
            try
            {
                var user = await _authService.RegisterAsync(registerDto);
                return CreatedAtAction(nameof(Register), new { id = user.Id }, new
                {
                    title = "Created",
                    status = 201,
                    detail = "User registered successfully",
                    instance = "/api/auth/register"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = ex.Message,
                    instance = "/api/auth/register"
                });
            }
        }

        /// <summary>
        /// Login user
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new
                {
                    title = "Unauthorized",
                    status = 401,
                    detail = "Invalid credentials",
                    instance = "/api/auth/login"
                });
            }
        }
    }
}