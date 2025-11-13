using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.ReviewsDTOs
{
    public class ReviewDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ProductId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedOnUtc { get; set; }

    }
}
