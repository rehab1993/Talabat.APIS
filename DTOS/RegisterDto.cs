using System.ComponentModel.DataAnnotations;

namespace Talabat.APIS.DTOS
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&amp;*()_+]).*$",
            ErrorMessage="Password must contain 1 Uppercase,1Lowercase,1Digit,1Special Character")]
        public string Password { get; set; }
    }
}
