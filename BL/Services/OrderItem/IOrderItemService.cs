using BLL.DTOs.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.OrderItem
{
    public interface IOrderItemService
    {
        public Task<List<OrderItemDto>> GetAllAsync();

    }
}
