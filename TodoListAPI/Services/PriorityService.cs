using TodoListAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListAPI.Repositories.Interfaces;

namespace TodoListAPI.Services
{
    public class PriorityService : IPriorityService
    {
        private readonly IPriorityRepository _repository;

        public PriorityService(IPriorityRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Priority>> GetAllPrioritiesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Priority?> GetPriorityByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Priority> CreatePriorityAsync(Priority priority)
        {
            return await _repository.CreateAsync(priority);
        }

        public async Task<Priority?> UpdatePriorityAsync(int id, Priority priority)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = priority.Name;

            return await _repository.UpdateAsync(existing);
        }

        public async Task<bool> DeletePriorityAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}