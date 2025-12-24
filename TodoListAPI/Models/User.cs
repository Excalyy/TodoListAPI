using System.ComponentModel.DataAnnotations;
using System.Data;
using TodoListAPI.Models.DTO;

namespace TodoListAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public List<TodoItem> TodoItems { get; set; } = [];
    }
}