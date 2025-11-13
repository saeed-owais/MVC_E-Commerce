using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.ReviewsDTOs
{
    public class CreateReviewDto
    {
        public string ProductId { get; set; }
        public string OrderId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // 1–5 stars
    }
}
