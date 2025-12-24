using TodoListAPI.Models;
using TodoListAPI.Models.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TodoListAPI.Repositories.Interfaces;

namespace TodoListAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
        }

        public async Task<User> RegisterAsync(RegisterRequestDto dto)
        {
            if (await _userRepository.GetByLoginAsync(dto.Login) != null)
                throw new ArgumentException("Login already exists");

            var role = await _roleRepository.GetByNameAsync(dto.Role) ?? await _roleRepository.GetByNameAsync("user");
            if (role == null) throw new ArgumentException("Invalid role");

            var user = new User
            {
                Login = dto.Login,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = role.Id
            };

            return await _userRepository.CreateAsync(user);
        }

        public async Task<object> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByLoginAsync(dto.Login);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "user")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(double.Parse(jwtSettings["ExpireHours"]!)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new { token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}