using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;
using GuestAPI.Models;
using RoomAPI.Models;

namespace BookingAPI.Data
{
    public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
    {

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Guest> Guests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>()
                .HasIndex(g => g.GuestEmail)
                .IsUnique();

            modelBuilder.Entity<Guest>()
                .HasIndex(g => g.GuestPhone)
                .IsUnique();
        }

        public DbSet<Room> Rooms { get; set; }
    }
}
