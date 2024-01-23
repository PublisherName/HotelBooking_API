using Microsoft.AspNetCore.Mvc;
using BookingAPI.Data;
using GuestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestAPI.Service
{
    public class GuestService(ApiContext context)
    {
        private readonly ApiContext _context = context;

        public async Task<ActionResult?> CheckUniqueEmailAndPhone(Guest guest)
        {
            var existingGuestEmail = await _context.Guests
                .Where(g => g.GuestEmail == guest.GuestEmail)
                .FirstOrDefaultAsync();

            if (existingGuestEmail != null)
            {
                return new BadRequestObjectResult(new { errors = "A guest with this email already exists" });
            }

            var existingGuestPhone = await _context.Guests
                .Where(g => g.GuestPhone == guest.GuestPhone)
                .FirstOrDefaultAsync();

            if (existingGuestPhone != null)
            {
                return new BadRequestObjectResult(new { errors = "A guest with this phone number already exists" });
            }

            return null;
        }

    }
}
