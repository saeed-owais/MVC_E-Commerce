using BLL.DTOs.Address;
using BLL.DTOs.CartItem;
using BLL.DTOs.Order;
using BLL.DTOs.OrderItem;
using DA.Models;

namespace E_Commerce_MVC.Models
{
    public class CheckoutViewModel
    {
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
        public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
        public CreateOrderDto CreateOrderDto { get; set; } 
        public decimal Total => CartItems.Sum(i => i.Total);
    }
}
