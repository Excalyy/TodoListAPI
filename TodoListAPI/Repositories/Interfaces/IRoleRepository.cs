using TodoListAPI.Models;
using System.Threading.Tasks;

namespace TodoListAPI.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByNameAsync(string name);
    }
}
