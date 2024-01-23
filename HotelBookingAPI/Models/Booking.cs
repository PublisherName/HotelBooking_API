using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingAPI.Attribute;

namespace BookingAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Room")]
        public int RoomId { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "Arrival date must be today or a future date")]
        public DateTime ArrivalDate { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "Departure Date must be today or a future date")]
        [DepartureDate("ArrivalDate", ErrorMessage = "Departure date must be greater than arrival date")]
        public DateTime DepartureDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of nights must be greater than 0")]
        public int NumberOfNights { get; set; }

        [Required]
        [ForeignKey("Guest")]
        public int GuestId { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Status must be 0 or 1")]
        public int Status { get; set; }
    }
}