using System.ComponentModel.DataAnnotations;

namespace API.ViewModel.Order
{
    public class CreateOrderViewModel
    {
        public string UserId { get; set; }
        public string AddressId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
