using TodoListAPI.Models;
using System.Threading.Tasks;

namespace TodoListAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByLoginAsync(string login);
        Task<User> CreateAsync(User entity);
        Task<User> UpdateAsync(User entity);
        Task<bool> DeleteAsync(int id);
    }
}