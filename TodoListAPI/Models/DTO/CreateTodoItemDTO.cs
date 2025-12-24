using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Models.DTO
{
    public class CreateTodoItemDTO
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        [Required]
        public int UserId { get; set; }

        public int PriorityId { get; set; } = 3; // Default Medium
    }
}