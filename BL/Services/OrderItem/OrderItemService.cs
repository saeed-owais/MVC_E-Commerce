using AutoMapper;
using BLL.DTOs.OrderItem;
using DA.Models;
using DAL.Interfaces;
namespace BLL.Services.OrderItem
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public OrderItemService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<List<OrderItemDto>> GetAllAsync()
        {
            var orderItems =  _uow.OrderItems.GetQueryable().Select
                (c => new OrderItemDto
                {
                    ProductId = c.Product.Id,
                    ProductName = c.Product.Name,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice
                });
            return _mapper.Map<List<OrderItemDto>>(orderItems);
        }
    }
}
