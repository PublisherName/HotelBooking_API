using Microsoft.AspNetCore.Mvc;
using GuestAPI.Models;
using BookingAPI.Data;

namespace GuestAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GuestController(ApiContext context) : ControllerBase
    {
        private readonly ApiContext _context = context;

        /// Creates a new guest or updates an existing one.
        [HttpPost]
        public async Task<ActionResult> CreateEdit(Guest guest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (guest.Id == 0)
            {
                _context.Guests.Add(guest);
            }
            else
            {
                var guestInDb = await _context.Guests.FindAsync(guest.Id);

                if (guestInDb == null)
                {
                    return NotFound();
                }

                if (await TryUpdateModelAsync<Guest>(
                    guestInDb,
                    "",
                    b => b.GuestName,
                    b => b.GuestSurname,
                    b => b.GuestEmail,
                    b => b.GuestPhone,
                    b => b.GuestAddress,
                    b => b.GuestCity,
                    b => b.GuestCountry,
                    b => b.ArrivalDate,
                    b => b.DepartureDate,
                    b => b.NumberOfNights
                    ))
                {
                    await _context.SaveChangesAsync();
                    return Ok(guest);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(guest);
        }

        // Shows the guest with the given id.
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _context.Guests.FindAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Deletes the guest with the given id.
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _context.Guests.FindAsync(id);

            if (result == null)
                return NotFound();

            _context.Guests.Remove(result);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Get all the guests.
        [HttpGet()]
        public JsonResult GetAll()
        {
            var result = _context.Guests.ToList();

            return new JsonResult(Ok(result));
        }
    }
}