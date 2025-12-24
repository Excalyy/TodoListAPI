using TodoListAPI.Models;
using TodoListAPI.Models.DTO;
using System.Threading.Tasks;

namespace TodoListAPI.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterRequestDto dto);
        Task<object> LoginAsync(LoginRequestDto dto);
    }
}