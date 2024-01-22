using System.ComponentModel.DataAnnotations;

namespace RoomAPI.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Room name must be between 2 and 50 characters")]
        public string? RoomName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Room description must be between 2 and 50 characters")]
        public string? RoomDescription { get; set; }

        [Required]
        [RegularExpression(@"^(single|double|villa)$", ErrorMessage = "Room type can be only single, double, or villa")]
        public string? RoomType { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of beds must be greater than 0")]
        public int RoomNoOfBed { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price per night must be greater than 0")]
        public int RoomPricePerNight { get; set; }

    }
}