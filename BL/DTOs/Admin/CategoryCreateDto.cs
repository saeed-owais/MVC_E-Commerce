using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs.Admin
{
    public class CategoryCreateDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
