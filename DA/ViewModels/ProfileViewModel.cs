using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    
    public class ProfileViewModel
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string? ImageURL { get; set; }
        public DateTime JoinDate { get; set; }
        public string Address { get; set; }
    }
}
