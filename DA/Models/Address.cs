using DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Models
{
    public class Address : BaseModel
    {
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        [Required, MaxLength(200)]
        public string Street { get; set; } = string.Empty;

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PostalCode { get; set; } = string.Empty;
    }
}
