using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs.Order
{
    public class CreateOrderDto
    {
        [Required]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string AddressId { get; set; }
        [Required(ErrorMessage = "Payment method is required")]
        public string PaymentMethod { get; set; }
    }
}
