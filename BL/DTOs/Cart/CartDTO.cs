using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOs.CartItem;
namespace BLL.DTOs.Cart
{
    public class CartDTO
    {
        public List<CartItemDTO> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Total);
    }
}
