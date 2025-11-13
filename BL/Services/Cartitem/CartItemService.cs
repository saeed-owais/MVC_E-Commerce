using AutoMapper;
using BLL.DTOs.CartItem;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Cartitem
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CartItemService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public Task<List<CartItemDTO>> GetAllAsync()
        {
            var cartItems =  _uow.CartItems.GetQueryable().Select(c => new CartItemDTO
            {
                ProductId = c.Product.Id,
                ProductName = c.Product.Name,
                Quantity = c.Quantity,
                Price = c.Product.Price
            });
            return Task.FromResult(cartItems.ToList());
        }
    }
}
