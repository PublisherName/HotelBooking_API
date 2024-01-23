using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Models
{

    public class Login
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be between 2 and 50 characters")]
        public string? Password { get; set; }

    }
}