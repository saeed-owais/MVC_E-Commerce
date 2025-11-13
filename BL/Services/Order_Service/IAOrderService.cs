using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOs.OrderDTOs;

namespace BLL.Services.Order_Service
{
    public interface IAOrderService
    {
        Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(string userId);
        Task<OrderDTO> GetOrderDetailsAsync(string orderId);
    }
}
