using AutoMapper;
using BLL.DTOs.Cart;
using BLL.DTOs.CartItem;
using DA.Models;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // 🟢 Get the current user's cart
        public async Task<CartDTO> GetCartAsync(string userId)
        {
            var items = await _unitOfWork.CartItems
                .GetQueryable()
                .Where(c => c.UserId == userId)
                .ProjectTo<CartItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new CartDTO
            {
                Items = items,
                // CartDto has Total property calculated automatically
            };
        }

        // 🟢 Add a product to cart
        public async Task AddItemToCartAsync(string userId, string productId, int qty)
        {
            var existingItem = await _unitOfWork.CartItems
                .GetQueryable()
                .Where(c => c.UserId == userId && c.ProductId == productId)
                .FirstOrDefaultAsync();

            if (existingItem != null)
            {
                existingItem.Quantity += qty;
                _unitOfWork.CartItems.Update(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ProductId = productId,
                    Quantity = qty
                };
                await _unitOfWork.CartItems.AddAsync(newItem);
            }

            await _unitOfWork.CompleteAsync();
        }

        // 🟢 Remove an item
        public async Task RemoveItemAsync(string cartItemId)
        {
            var item = await _unitOfWork.CartItems.GetByIdAsync(cartItemId);
            if (item == null)
                throw new KeyNotFoundException($"Cart item {cartItemId} not found");

            _unitOfWork.CartItems.Remove(item);
            await _unitOfWork.CompleteAsync();
        }

        // 🟢 Update item quantity
        public async Task UpdateQuantityAsync(string cartItemId, int qty)
        {
            var item = await _unitOfWork.CartItems.GetByIdAsync(cartItemId);
            if (item == null)
                throw new KeyNotFoundException($"Cart item {cartItemId} not found");

            item.Quantity = qty;
            _unitOfWork.CartItems.Update(item);
            await _unitOfWork.CompleteAsync();
        }
    }
}
