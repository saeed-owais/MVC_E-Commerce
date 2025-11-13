using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs.Admin
{
    public class CategoryAdminDto
    {
        public string Id { get; set; } = string.Empty;

        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
