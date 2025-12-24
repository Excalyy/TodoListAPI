using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListAPI.Data;
using TodoListAPI.Models;
using TodoListAPI.Repositories.Interfaces;

namespace TodoListAPI.Repositories
{
    public class PriorityRepository : IPriorityRepository
    {
        private readonly AppDbContext _context;

        public PriorityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Priority>> GetAllAsync()
        {
            return await _context.Priorities.ToListAsync();
        }

        public async Task<Priority?> GetByIdAsync(int id)
        {
            return await _context.Priorities.FindAsync(id);
        }

        public async Task<Priority> CreateAsync(Priority entity)
        {
            _context.Priorities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Priority> UpdateAsync(Priority entity)
        {
            _context.Priorities.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var priority = await GetByIdAsync(id);
            if (priority == null) return false;
            _context.Priorities.Remove(priority);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}