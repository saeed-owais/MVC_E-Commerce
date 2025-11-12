using BLL.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Cart
{
    public interface ICartService
    {
        Task<CartDTO> GetCartAsync(string userId);
        Task AddItemToCartAsync(string userId, string productId, int quantity);
        Task RemoveItemAsync(string cartItemId);
        Task UpdateQuantityAsync(string cartItemId, int quantity);
    }
}
