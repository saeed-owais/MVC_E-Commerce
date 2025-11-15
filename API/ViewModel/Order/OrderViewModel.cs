using BLL.DTOs.OrderItem;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModel.Order
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public string? PaymentMethod { get; set; }
        public bool PaymentSuccess { get; set; }
    }
}
