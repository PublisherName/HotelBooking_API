using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;
using GuestAPI.Models;

namespace BookingAPI.Data
{
    public class ApiContext : DbContext
    {

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Guest> Guests { get; set; }


        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {

        }

    }
}
