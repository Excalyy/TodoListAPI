using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TodoListAPI.Services;
using TodoListAPI.Models.DTO;
using TodoListAPI.Models;
using AutoMapper;
using System.Security.Claims;

namespace TodoListAPI.Controller  
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _service;
        private readonly IMapper _mapper;

        public TodoItemsController(ITodoItemService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all todo items (admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetTodoItems()
        {
            var items = await _service.GetAllTodoItemsAsync();
            var dtos = _mapper.Map<List<TodoItemDTO>>(items);
            return Ok(dtos);
        }

        /// <summary>
        /// Get todo item by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetTodoItem(int id)
        {
            var item = await _service.GetTodoItemByIdAsync(id);
            if (item == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Todo item with ID {id} not found.",
                    instance = $"/api/todoitems/{id}"
                });

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentRole != "admin" && item.UserId != currentUserId)
                return Forbid();

            var dto = _mapper.Map<TodoItemDTO>(item);
            return Ok(dto);
        }

        /// <summary>
        /// Get todo items for a user
        /// </summary>
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetUserTodoItems(int userId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentRole != "admin" && userId != currentUserId)
                return Forbid();

            var items = await _service.GetTodoItemsByUserIdAsync(userId);
            var dtos = _mapper.Map<List<TodoItemDTO>>(items);
            return Ok(dtos);
        }

        /// <summary>
        /// Create new todo item
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> CreateTodoItem(CreateTodoItemDTO createDto)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentRole != "admin" && createDto.UserId != currentUserId)
                return Forbid();

            var item = _mapper.Map<TodoItem>(createDto);
            TodoItem createdItem;
            try
            {
                createdItem = await _service.CreateTodoItemAsync(item);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = ex.Message,
                    instance = "/api/todoitems"
                });
            }

            var dto = _mapper.Map<TodoItemDTO>(createdItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Update todo item
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> UpdateTodoItem(int id, UpdateTodoItemDTO updateDto)
        {
            var item = await _service.GetTodoItemByIdAsync(id);
            if (item == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Todo item with ID {id} not found.",
                    instance = $"/api/todoitems/{id}"
                });

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentRole != "admin" && item.UserId != currentUserId)
                return Forbid();

            var mapped = _mapper.Map<TodoItem>(updateDto);
            var updatedItem = await _service.UpdateTodoItemAsync(id, mapped);
            if (updatedItem == null)
                return NotFound();

            var dto = _mapper.Map<TodoItemDTO>(updatedItem);
            return Ok(dto);
        }

        /// <summary>
        /// Delete todo item (admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var item = await _service.GetTodoItemByIdAsync(id);
            if (item == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Todo item with ID {id} not found.",
                    instance = $"/api/todoitems/{id}"
                });

            var deletedInfo = new
            {
                Id = item.Id,
                Title = item.Title,
                DeletedAt = DateTime.UtcNow
            };

            var result = await _service.DeleteTodoItemAsync(id);
            if (!result)
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = $"Failed to delete todo item with ID {id}.",
                    instance = $"/api/todoitems/{id}"
                });

            return Ok(new
            {
                message = "Todo item deleted successfully",
                deletedItem = deletedInfo
            });
        }
    }
}