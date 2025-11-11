using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs.Admin
{
    public class ProductAdminDto
    {
        public string Id { get; set; } = string.Empty;

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 100000.00, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Stock must be 0 or more")]
        public int Stock { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select a category")]
        public string CategoryId { get; set; } = string.Empty;

        public string? CategoryName { get; set; }
    }
}
