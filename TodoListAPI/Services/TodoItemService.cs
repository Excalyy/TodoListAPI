using TodoListAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListAPI.Repositories.Interfaces;

namespace TodoListAPI.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IPriorityRepository _priorityRepository;

        public TodoItemService(ITodoItemRepository repository, IUserRepository userRepository, IPriorityRepository priorityRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _priorityRepository = priorityRepository;
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TodoItem?> GetTodoItemByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem)
        {
            var user = await _userRepository.GetByIdAsync(todoItem.UserId);
            if (user == null) throw new ArgumentException("User not found");

            var priority = await _priorityRepository.GetByIdAsync(todoItem.PriorityId);
            if (priority == null) throw new ArgumentException("Priority not found");

            todoItem.CreatedAt = DateTime.UtcNow;
            return await _repository.CreateAsync(todoItem);
        }

        public async Task<TodoItem?> UpdateTodoItemAsync(int id, TodoItem todoItem)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Title = todoItem.Title ?? existing.Title;
            existing.Description = todoItem.Description ?? existing.Description;
            existing.IsCompleted = todoItem.IsCompleted;
            existing.DueDate = todoItem.DueDate ?? existing.DueDate;
            existing.PriorityId = todoItem.PriorityId != 0 ? todoItem.PriorityId : existing.PriorityId;

            return await _repository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteTodoItemAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsByUserIdAsync(int userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }
    }
}