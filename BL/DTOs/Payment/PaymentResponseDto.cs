using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Payment
{
    public class PaymentResponseDto
    {
        public bool IsSuccessful { get; set; }
        public string? TransactionId { get; set; }
        public string? Message { get; set; }
    }
}
