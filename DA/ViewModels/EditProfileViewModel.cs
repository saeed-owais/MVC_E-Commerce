using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(6, ErrorMessage = "Name must be at least 6 characters long")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }




        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }


        public IFormFile? ImageFile { get; set; }   
        public string? ExistingImageURL { get; set; }
    }
}
