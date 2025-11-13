using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DA.Models
{
    public class OrderItem : BaseModel
    {
        public string OrderId { get; set; }
        public Order? Order { get; set; }

        public string ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        [Precision(18, 2)]

        public decimal UnitPrice { get; set; }
    }

}
