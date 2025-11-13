using BLL.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Order
{
    public interface IOrderService
    {
        public Task<(bool Success, string Message, OrderDto? Order)> CreateOrderFromCartAsync(CreateOrderDto dto);

    }
}
