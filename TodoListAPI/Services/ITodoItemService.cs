using TodoListAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoListAPI.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync();
        Task<TodoItem?> GetTodoItemByIdAsync(int id);
        Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem);
        Task<TodoItem?> UpdateTodoItemAsync(int id, TodoItem todoItem);
        Task<bool> DeleteTodoItemAsync(int id);
        Task<IEnumerable<TodoItem>> GetTodoItemsByUserIdAsync(int userId);
    }
}