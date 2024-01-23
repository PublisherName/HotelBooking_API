using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;
using BookingAPI.Data;

namespace BookingAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookingController(ApiContext context) : ControllerBase
    {
        private readonly ApiContext _context = context;

        /// Creates a new booking or updates an existing one.
        [HttpPost]
        public async Task<ActionResult> CreateEdit(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
            }

            var room = await _context.Rooms.FindAsync(booking.RoomId);
            if (room == null)
            {
                return BadRequest(new { errors = "The room id does not exist" });
            }

            var guest = await _context.Guests.FindAsync(booking.GuestId);
            if (guest == null)
            {
                return BadRequest(new { errors = "The guest id does not exist" });
            }

            if (booking.Id == 0)
            {
                _context.Bookings.Add(booking);
            }
            else
            {
                var bookingInDb = await _context.Bookings.FindAsync(booking.Id);

                if (bookingInDb == null)
                {
                    return BadRequest(new { errors = "The booking id does not exist" });
                }

                if (!await TryUpdateModelAsync<Booking>(
                    bookingInDb,
                    "",
                    b => b.ArrivalDate,
                    b => b.DepartureDate,
                    b => b.NumberOfNights,
                    b => b.RoomId,
                    b => b.GuestId,
                    b => b.Status
                    ))
                {
                    return BadRequest(new { errors = "Failed to update the booking" });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = $"An error occurred while saving changes: {ex.Message}" });
            }

            return Ok(booking);
        }

        // Shows the booking with the given id.
        [HttpGet("{guestId}")]
        public async Task<IActionResult> GetByGuestId(int guestId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.GuestId == guestId)
                .ToListAsync();

            if (bookings.Count == 0)
                return BadRequest(new { errors = "No booking record found." });

            return Ok(bookings);
        }

        // Deletes the booking with the given id.
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _context.Bookings.FindAsync(id);

            if (result == null)
                return BadRequest(new { errors = "The booking id does not exist" });

            _context.Bookings.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Get all the bookings.
        [HttpGet()]
        public JsonResult GetAll()
        {
            var result = _context.Bookings.ToList();

            return new JsonResult(Ok(result));
        }

    }
}