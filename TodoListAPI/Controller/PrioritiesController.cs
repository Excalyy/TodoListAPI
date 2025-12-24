using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TodoListAPI.Services;
using TodoListAPI.Models.DTO;
using TodoListAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore; // Добавил
using Microsoft.Data.Sqlite; // Можно удалить если не используется

namespace TodoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")] // Весь контроллер требует роль admin
    public class PrioritiesController : ControllerBase
    {
        private readonly IPriorityService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<PrioritiesController> _logger;

        public PrioritiesController(
            IPriorityService service,
            IMapper mapper,
            ILogger<PrioritiesController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all priorities
        /// </summary>
        [HttpGet]
        [AllowAnonymous] // Доступно всем для чтения
        public async Task<IActionResult> GetPriorities()
        {
            try
            {
                var priorities = await _service.GetAllPrioritiesAsync();
                var dtos = _mapper.Map<List<PriorityDTO>>(priorities);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting priorities");
                return StatusCode(500, new
                {
                    title = "Internal Server Error",
                    status = 500,
                    detail = "An error occurred while retrieving priorities.",
                    instance = "/api/priorities"
                });
            }
        }

        /// <summary>
        /// Get priority by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPriority(int id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting priority with ID {PriorityId}", id);
                return StatusCode(500, new
                {
                    title = "Internal Server Error",
                    status = 500,
                    detail = $"An error occurred while retrieving priority with ID {id}.",
                    instance = $"/api/priorities/{id}"
                });
            }
        }

        /// <summary>
        /// Create new priority (admin only)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePriority(CreatePriorityDTO createDto)
        {
            try
            {
                var priority = _mapper.Map<Priority>(createDto);
                var created = await _service.CreatePriorityAsync(priority);
                var dto = _mapper.Map<PriorityDTO>(created);
                return CreatedAtAction(nameof(GetPriority), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating priority");
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = ex.Message,
                    instance = "/api/priorities"
                });
            }
        }

        /// <summary>
        /// Update priority (admin only)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePriority(int id, UpdatePriorityDTO updateDto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating priority with ID {PriorityId}", id);
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = ex.Message,
                    instance = $"/api/priorities/{id}"
                });
            }
        }

        /// <summary>
        /// Delete priority (admin only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriority(int id)
        {
            try
            {
                // 1. Сначала проверяем существование
                var priority = await _service.GetPriorityByIdAsync(id);
                if (priority == null)
                {
                    return NotFound(new
                    {
                        title = "Not Found",
                        status = 404,
                        detail = $"Priority with ID {id} not found.",
                        instance = $"/api/priorities/{id}"
                    });
                }

                // 2. Пытаемся удалить
                var result = await _service.DeletePriorityAsync(id);

                if (!result)
                {
                    return BadRequest(new
                    {
                        title = "Bad Request",
                        status = 400,
                        detail = $"Failed to delete priority with ID {id}.",
                        instance = $"/api/priorities/{id}"
                    });
                }

                // 3. Возвращаем успешный ответ
                return Ok(new
                {
                    message = "Priority deleted successfully",
                    deletedPriority = new
                    {
                        Id = priority.Id,
                        Name = priority.Name,
                        DeletedAt = DateTime.UtcNow
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                // Обработка ошибки "нельзя удалить, потому что используется"
                _logger.LogWarning(ex, "Cannot delete priority {PriorityId}", id);
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = ex.Message,
                    instance = $"/api/priorities/{id}"
                });
            }
            catch (DbUpdateException dbEx)
            {
                // Обработка ошибок БД (например, нарушение foreign key)
                _logger.LogError(dbEx, "Database error deleting priority {PriorityId}", id);

                var errorMessage = dbEx.InnerException?.Message ?? dbEx.Message;

                return BadRequest(new
                {
                    title = "Database Error",
                    status = 400,
                    detail = $"Cannot delete priority because it is in use. Details: {errorMessage}",
                    instance = $"/api/priorities/{id}"
                });
            }
            catch (Exception ex)
            {
                // Общая обработка ошибок
                _logger.LogError(ex, "Error deleting priority with ID {PriorityId}", id);
                return StatusCode(500, new
                {
                    title = "Internal Server Error",
                    status = 500,
                    detail = $"An error occurred while deleting priority with ID {id}.",
                    instance = $"/api/priorities/{id}"
                });
            }
        }

        /// <summary>
        /// Safe delete - проверяет можно ли удалить приоритет
        /// </summary>
        [HttpGet("can-delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CanDeletePriority(int id)
        {
            try
            {
                var priority = await _service.GetPriorityByIdAsync(id);
                if (priority == null)
                {
                    return Ok(new
                    {
                        canDelete = false,
                        reason = $"Priority with ID {id} does not exist."
                    });
                }

                // Временно возвращаем true для тестирования
                return Ok(new
                {
                    canDelete = true,
                    priority = new
                    {
                        priority.Id,
                        priority.Name
                        // Color и Order удалены, так как их нет в модели
                    },
                    warning = "This will permanently delete the priority."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if priority {PriorityId} can be deleted", id);
                return Ok(new
                {
                    canDelete = false,
                    reason = $"Error checking: {ex.Message}"
                });
            }
        }
    }
}