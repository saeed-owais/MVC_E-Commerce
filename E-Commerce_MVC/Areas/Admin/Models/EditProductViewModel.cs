using System.ComponentModel.DataAnnotations;

namespace E_Commerce_MVC.Areas.Admin.Models
{
    public class EditProductViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        public string? ExistingImageUrl { get; set; }

        [Display(Name = "Product Name")]
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 100000.00)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 10000)]
        public int Stock { get; set; }

        [Display(Name = "Category")]
        [Required]
        public string CategoryId { get; set; } = string.Empty;

        [Display(Name = "Product Image")]
        public IFormFile? ImageFile { get; set; }
    }
}