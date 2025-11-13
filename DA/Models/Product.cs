using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class Product : BaseModel
    {

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        public string CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Review>? Reviews { get; set; }
    }
}
