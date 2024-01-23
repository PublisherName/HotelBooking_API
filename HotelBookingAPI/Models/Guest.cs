using System.ComponentModel.DataAnnotations;

namespace GuestAPI.Models
{
    public class Guest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        public string? GuestName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Surname must be between 2 and 50 characters")]
        public string? GuestSurname { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        public string? GuestEmail { get; set; }

        [Required]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Phone number must be a country code plus 10 digits")]
        public string? GuestPhone { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Guest address must be between 2 and 50 characters")]
        public string? GuestAddress { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "City must be between 2 and 50 characters")]
        public string? GuestCity { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Country must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Country must only contain letters and spaces")]
        public string? GuestCountry { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be between 2 and 50 characters")]
        public string? GuestPassword { get; set; }
    }
}