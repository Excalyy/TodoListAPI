using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListAPI.Data;
using TodoListAPI.Models;
using TodoListAPI.Repositories.Interfaces;

namespace TodoListAPI.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly AppDbContext _context;

        public TodoItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .Include(t => t.Priority)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .Include(t => t.Priority)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TodoItem> CreateAsync(TodoItem entity)
        {
            _context.TodoItems.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem entity)
        {
            _context.TodoItems.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await GetByIdAsync(id);
            if (item == null) return false;
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TodoItem>> GetByUserIdAsync(int userId)
        {
            return await _context.TodoItems
                .Include(t => t.User)
                .Include(t => t.Priority)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
    }
}