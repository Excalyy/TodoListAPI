using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoListAPI.Data;
using TodoListAPI.Models;
using TodoListAPI.Repositories.Interfaces;
namespace TodoListAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<User> CreateAsync(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}