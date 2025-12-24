using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Models.DTO
{
    public class CreatePriorityDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}