using Microsoft.AspNetCore.Mvc;
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
                return BadRequest(ModelState);

            if (booking.Id == 0)
            {
                _context.Bookings.Add(booking);
            }
            else
            {
                var bookingInDb = await _context.Bookings.FindAsync(booking.Id);

                if (bookingInDb == null)
                {
                    return NotFound();
                }

                if (await TryUpdateModelAsync<Booking>(
                    bookingInDb,
                    "",
                    b => b.RoomNumber, b => b.ClientName))
                {
                    await _context.SaveChangesAsync();
                    return Ok(booking);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        // Shows the booking with the given id.
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _context.Bookings.FindAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Deletes the booking with the given id.
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _context.Bookings.FindAsync(id);

            if (result == null)
                return NotFound();

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