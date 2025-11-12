using BLL.DTOs.Address;
using BLL.DTOs.CartItem;
using BLL.DTOs.Order;
using BLL.DTOs.OrderItem;
using DA.Models;

namespace E_Commerce_MVC.Models
{
    public class CheckoutViewModel
    {
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
        public CreateOrderDto CreateOrderDto { get; set; } 
        public decimal Total => CartItems.Sum(i => i.TotalPrice);
    }
}
