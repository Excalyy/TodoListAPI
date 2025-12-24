using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime? DueDate { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int PriorityId { get; set; }
        public Priority? Priority { get; set; }
    }
}