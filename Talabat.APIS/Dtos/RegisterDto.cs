using System.ComponentModel.DataAnnotations;

namespace Talabat.APIS.Dtos
{
    public class RegisterDto
    {
        [Required]
        
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%&amp;*()_+]).*$",
            ErrorMessage ="Password Must be Contains 1 Uppercase ,1 lowercase, 1 digit, 1 speacial character !")]
        public string Password { get; set; }

    }
}
