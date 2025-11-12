using BLL.DTOs.CartItem;

namespace E_Commerce_MVC.Models.Cart
{
    public class CartViewModel
    {
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal Total => Items.Sum(i => i.Price * i.Quantity);
    }
}
