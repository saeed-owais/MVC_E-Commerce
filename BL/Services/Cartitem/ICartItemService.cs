using BLL.DTOs.CartItem;

namespace BLL.Services.Cartitem
{
    public interface ICartItemService
    {
        Task<List<CartItemDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<CartItemDTO>> GetByUserAsync(string userId , CancellationToken cancellationToken = default);
    }
}
