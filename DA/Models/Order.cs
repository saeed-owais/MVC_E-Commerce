using DAL.Enums;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class Order : BaseModel
    {
        [Required]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Pending / Shipped / Delivered / Cancelled
        [Precision(18, 2)]

        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }

        public Payment? Payment { get; set; }
    }
}
