using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Models.DTO
{
    public class UpdateTodoItemDTO
    {
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public DateTime? DueDate { get; set; }

        public int PriorityId { get; set; }
    }
}