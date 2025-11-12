using System.ComponentModel.DataAnnotations;

namespace E_Commerce_MVC.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email Isn't Valid")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remmember Me")]
        public Boolean Remmember { get; set; }
    }
}
