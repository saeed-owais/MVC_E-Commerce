using BLL.Services.Cart;
using DA.Models;
using E_Commerce_MVC.Models.Cart;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager; 
        }

        // In a real app, you’ll get this from the authenticated user
        private string GetUserId() => _userManager.GetUserId(User);

        // 🟢 GET: /Cart
        public async Task<IActionResult> Index()
        {
            var cartDto = await _cartService.GetCartAsync(GetUserId());

            var viewModel = new CartViewModel
            {
                Items = cartDto.Items.ToList()
            };

            return View(viewModel);
        }

        // 🟢 POST: /Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int qty)
        {
            if (string.IsNullOrEmpty(productId) || qty <= 0)
                return BadRequest();

            await _cartService.AddItemToCartAsync(GetUserId(), productId, qty);

            return RedirectToAction("Index", "Cart");
        }

        // 🟢 POST: /Cart/Remove
        [HttpPost]
        public async Task<IActionResult> Remove(string cartItemId)
        {
            if (string.IsNullOrEmpty(cartItemId))
                return BadRequest();

            await _cartService.RemoveItemAsync(cartItemId);

            return RedirectToAction("Index");
        }

        // 🟢 POST: /Cart/UpdateQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(string cartItemId, int qty)
        {
            if (string.IsNullOrEmpty(cartItemId) || qty < 1)
                return BadRequest();

            await _cartService.UpdateQuantityAsync(cartItemId, qty);

            return RedirectToAction("Index");
        }
    }
}
