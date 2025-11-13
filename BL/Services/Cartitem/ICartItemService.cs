using BLL.DTOs.CartItem;

namespace BLL.Services.Cartitem
{
    public interface ICartItemService
    {
        Task<List<CartItemDTO>> GetAllAsync();
    }
}
