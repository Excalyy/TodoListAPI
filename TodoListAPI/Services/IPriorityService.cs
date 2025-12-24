using TodoListAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoListAPI.Services
{
    public interface IPriorityService
    {
        Task<IEnumerable<Priority>> GetAllPrioritiesAsync();
        Task<Priority?> GetPriorityByIdAsync(int id);
        Task<Priority> CreatePriorityAsync(Priority priority);
        Task<Priority?> UpdatePriorityAsync(int id, Priority priority);
        Task<bool> DeletePriorityAsync(int id);
    }
}
