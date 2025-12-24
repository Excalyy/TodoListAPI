using TodoListAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoListAPI.Repositories.Interfaces
{
    public interface ITodoItemRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(int id);
        Task<TodoItem> CreateAsync(TodoItem entity);
        Task<TodoItem> UpdateAsync(TodoItem entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TodoItem>> GetByUserIdAsync(int userId);
    }
}
