using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TodoListAPI.Services;
using TodoListAPI.Models.DTO;
using TodoListAPI.Models;
using AutoMapper;

namespace TodoListAPI.Controller 
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class PrioritiesController : ControllerBase
    {
        private readonly IPriorityService _service;
        private readonly IMapper _mapper;

        public PrioritiesController(IPriorityService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all priorities
        /// </summary>
        [HttpGet]
        [AllowAnonymous] // Доступно всем для чтения
        public async Task<IActionResult> GetPriorities()
        {
            var priorities = await _service.GetAllPrioritiesAsync();
            var dtos = _mapper.Map<List<PriorityDTO>>(priorities);
            return Ok(dtos);
        }

        /// <summary>
        /// Get priority by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPriority(int id)
        {
            var priority = await _service.GetPriorityByIdAsync(id);
            if (priority == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Priority with ID {id} not found.",
                    instance = $"/api/priorities/{id}"
                });

            var dto = _mapper.Map<PriorityDTO>(priority);
            return Ok(dto);
        }

        /// <summary>
        /// Create new priority (admin only)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePriority(CreatePriorityDTO createDto)
        {
            var priority = _mapper.Map<Priority>(createDto);
            var created = await _service.CreatePriorityAsync(priority);
            var dto = _mapper.Map<PriorityDTO>(created);
            return CreatedAtAction(nameof(GetPriority), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Update priority (admin only)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePriority(int id, UpdatePriorityDTO updateDto)
        {
            var mapped = _mapper.Map<Priority>(updateDto);
            var updated = await _service.UpdatePriorityAsync(id, mapped);
            if (updated == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Priority with ID {id} not found.",
                    instance = $"/api/priorities/{id}"
                });

            var dto = _mapper.Map<PriorityDTO>(updated);
            return Ok(dto);
        }

        /// <summary>
        /// Delete priority (admin only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriority(int id)
        {
            var priority = await _service.GetPriorityByIdAsync(id);
            if (priority == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Priority with ID {id} not found.",
                    instance = $"/api/priorities/{id}"
                });

            var deletedInfo = new
            {
                Id = priority.Id,
                Name = priority.Name,
                DeletedAt = DateTime.UtcNow
            };

            var result = await _service.DeletePriorityAsync(id);
            if (!result)
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = $"Failed to delete priority with ID {id}.",
                    instance = $"/api/priorities/{id}"
                });

            return Ok(new
            {
                message = "Priority deleted successfully",
                deletedPriority = deletedInfo
            });
        }
    }
}