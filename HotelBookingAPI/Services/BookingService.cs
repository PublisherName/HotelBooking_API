using Microsoft.AspNetCore.Mvc;
using BookingAPI.Data;
using BookingAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace BookingAPI.Service
{
    public class BookingService(ApiContext context)
    {
        private readonly ApiContext _context = context;
        public async Task<ActionResult?> ValidateBookingEntitiesExist(Booking booking)
        {
            var room = await _context.Rooms.FindAsync(booking.RoomId);
            if (room == null)
            {
                return new BadRequestObjectResult(new { errors = "The room id does not exist" });
            }

            var guest = await _context.Guests.FindAsync(booking.GuestId);
            if (guest == null)
            {
                return new BadRequestObjectResult(new { errors = "The guest id does not exist" });
            }

            return null;
        }

        public async Task<ActionResult?> UpdateBooking(Booking booking)
        {
            var bookingInDb = await _context.Bookings.FindAsync(booking.Id);

            if (bookingInDb == null)
            {
                return new BadRequestObjectResult(new { errors = "The booking id does not exist" });
            }

            bookingInDb.ArrivalDate = booking.ArrivalDate;
            bookingInDb.DepartureDate = booking.DepartureDate;
            bookingInDb.NumberOfNights = booking.NumberOfNights;
            bookingInDb.RoomId = booking.RoomId;
            bookingInDb.GuestId = booking.GuestId;
            bookingInDb.Status = booking.Status;

            return null;
        }

        public async Task<ActionResult?> CheckRoomAvailability(Booking booking)
        {
            var overlappingBookings = await _context.Bookings
                .Where(b => b.RoomId == booking.RoomId &&
                            b.ArrivalDate < booking.DepartureDate &&
                            b.DepartureDate > booking.ArrivalDate)
                .ToListAsync();

            if (overlappingBookings.Count != 0)
            {
                var sameGuestBooking = overlappingBookings.FirstOrDefault(b => b.GuestId == booking.GuestId);
                if (sameGuestBooking != null)
                {
                    return new BadRequestObjectResult(new
                    {
                        errors = $"You have already booked this room from {sameGuestBooking.ArrivalDate.ToShortDateString()} to {sameGuestBooking.DepartureDate.ToShortDateString()}"
                    });
                }
                else
                {
                    var nextVacantDate = overlappingBookings.Max(b => b.DepartureDate);
                    return new BadRequestObjectResult(new
                    {
                        errors = $"The room is not available for the selected dates. It will be vacant from {nextVacantDate.AddDays(1).ToShortDateString()}"

                    });
                }
            }
            return null;
        }

        public async Task<ActionResult?> SaveChanges()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { errors = $"An error occurred while saving changes: {ex.Message}" });
            }

            return null;
        }
    }
}