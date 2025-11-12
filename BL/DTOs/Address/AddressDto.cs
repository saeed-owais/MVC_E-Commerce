using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Address
{
    public class AddressDto
    {
        public string? Id { get; set; }

        public string? UserId { get; set; }

        
        public string Street { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }
    }
}
