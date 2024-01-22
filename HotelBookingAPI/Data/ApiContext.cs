using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;
using GuestAPI.Models;
using RoomAPI.Models;

namespace BookingAPI.Data
{
    public class ApiContext : DbContext
    {

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Guest> Guests { get; set; }

        public DbSet<Room> Rooms { get; set; }


        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {

        }

    }
}
