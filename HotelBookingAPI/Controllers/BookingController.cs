using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;
using BookingAPI.Data;
using BookingAPI.Service;

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
                return BadRequest(ModelState);

            var validateEntities = await new BookingService(_context).ValidateBookingEntitiesExist(booking);

            if (validateEntities is BadRequestObjectResult badRequest)
                return BadRequest(badRequest.Value);

            if (booking.Id != 0)
            {
                var updateBooking = await new BookingService(_context).UpdateBooking(booking);
                if (updateBooking is BadRequestObjectResult badRequestUpdate)
                    return BadRequest(badRequestUpdate.Value);
            }
            else
                _context.Bookings.Add(booking);

            var saveChanges = await new BookingService(_context).SaveChanges();
            if (saveChanges is BadRequestObjectResult badRequestSave)
            {
                return BadRequest(badRequestSave.Value);
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