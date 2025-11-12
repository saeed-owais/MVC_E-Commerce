using DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class BaseModel : IAuditable, ISoftDelete
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // IAuditable Implementation
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
        public string? CreatedById { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
        public string? ModifiedById { get; set; }

        // ISoftDelete Implementation
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedOnUtc { get; set; }
        public string? DeletedById { get; set; }
    }
}
