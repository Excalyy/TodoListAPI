using TodoListAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoListAPI.Repositories.Interfaces
{
    public interface IPriorityRepository
    {
        Task<IEnumerable<Priority>> GetAllAsync();
        Task<Priority?> GetByIdAsync(int id);
        Task<Priority> CreateAsync(Priority entity);
        Task<Priority> UpdateAsync(Priority entity);
        Task<bool> DeleteAsync(int id);
    }
}