using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Models.DTO
{
    public class UpdatePriorityDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}