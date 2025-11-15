using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs.Account
{

    public enum Gender
    {
        Male = 1,
        Female = 2
    }

    public class RegisterDto
    {
        [Required]
        [MinLength(6)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

}
