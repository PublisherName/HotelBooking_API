using Microsoft.AspNetCore.Mvc;
using RoomAPI.Models;
using BookingAPI.Data;

namespace RoomAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoomController(ApiContext context) : ControllerBase
    {
        private readonly ApiContext _context = context;

        [HttpPost]
        public async Task<ActionResult> CreateEdit(Room room)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (room.Id == 0)
            {
                _context.Rooms.Add(room);
            }
            else
            {
                var roomInDb = await _context.Rooms.FindAsync(room.Id);

                if (roomInDb == null)
                {
                    return NotFound();
                }

                if (await TryUpdateModelAsync<Room>(
                    roomInDb,
                    "",
                    b => b.RoomName,
                    b => b.RoomDescription,
                    b => b.RoomType,
                    b => b.RoomNoOfBed,
                    b => b.RoomPricePerNight
                    ))
                {
                    await _context.SaveChangesAsync();
                    return Ok(room);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(room);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet()]
        public JsonResult GetAll()
        {
            var result = _context.Rooms.ToList();

            return new JsonResult(Ok(result));
        }

    }
}