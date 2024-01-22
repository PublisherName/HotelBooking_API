using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;

namespace BookingAPI.Data
{
    public class ApiContext : DbContext
    {

        public DbSet<Booking> Bookings { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {

        }

    }
}