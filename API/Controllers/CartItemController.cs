using API.Response;
using API.ViewModel.CartItem;
using BLL.Services.Cartitem;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }


        [HttpGet("user/{userId}")]
        public async Task<ApiResponse<List<AllCartItemViewModel>>> GetByUser(string userId)
        {
            var cartItems = await _cartItemService.GetByUserAsync(userId);
            var cartItemsviewModel = cartItems.Select(ci => new AllCartItemViewModel
            {
                ProductId = ci.ProductId,
                ProductName = ci.ProductName,
                Price = ci.Price,
                Quantity = ci.Quantity
            }).ToList();

            return ResponseHelper.Success(cartItemsviewModel);
        }
    }
}
