using AutoMapper;
using BLL.DTOs.Order;
using BLL.Services.Order;
using DA.Models;
using DAL.Enums;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<(bool Success, string Message, OrderDto? Order)> CreateOrderFromCartAsync(CreateOrderDto dto)
    {
        var cart = await _uow.CartItems
               .GetQueryable()
               .Where(c => c.UserId == dto.UserId)
               .Select(c => new
               {
                   c.ProductId,
                   c.Quantity,
                   ProductName = c.Product.Name,
                   ProductPrice = c.Product.Price
               })
               .ToListAsync();
        if (!cart.Any())
            return (false, "Cart is empty", null);

        var order = new Order
        {
            UserId = dto.UserId,
            TotalAmount = cart.Sum(x => x.Quantity * x.ProductPrice),
            Status = OrderStatus.Pending
        };

        await _uow.Orders.AddAsync(order);
        await _uow.CompleteAsync();

        foreach (var item in cart)
        {
            await _uow.OrderItems.AddAsync(new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.ProductPrice
            });
        }

        await _uow.CompleteAsync();
        var cartEntities = await _uow.CartItems
       .GetQueryable()
       .Where(c => c.UserId == dto.UserId)
       .ToListAsync();

        foreach (var item in cartEntities)
            _uow.CartItems.Remove(item);

        await _uow.CompleteAsync();

        return (true, "Order created", _mapper.Map<OrderDto>(order));
    }

}
