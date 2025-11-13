using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOs.OrderItemsDTOs;

namespace BLL.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemDTO> OrderItems { get; set; }
    }
}
