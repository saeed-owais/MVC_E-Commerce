using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class ApplicationUser : IdentityUser, IAuditable, ISoftDelete
    {
        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;
        public string? ImageURL { get; set; }
        public string Address { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review>? Reviews { get; set; }

        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
        public string? CreatedById { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
        public string? ModifiedById { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedOnUtc { get; set; }
        public string? DeletedById { get; set; }
    }
}
