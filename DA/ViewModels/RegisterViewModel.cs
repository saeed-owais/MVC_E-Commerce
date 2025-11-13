using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_MVC.ViewModels
{
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [MinLength(6, ErrorMessage = "Full Name must be at least 6 characters long")]
        public string FullName { get; set; }

        //[Required]
        //public string LastName { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassowrd {  get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string  Email { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")] 
        public string Phone { get ; set; }

        [Required(ErrorMessage = "Field is required")]
        public string Country { get ; set; }


    }
}
