using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI.Models
{
    public class HotelBooking
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "RoomNumber must be greater than 0")]
        public int RoomNumber { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "ClientName must be between 2 and 50 characters")]
        public string? ClientName { get; set; }

    }
}