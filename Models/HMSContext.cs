using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HMS.Models
{
    public class HMSContext : IdentityDbContext<User>
    {
        public HMSContext(DbContextOptions<HMSContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>().HasOne(r => r.Room)
                .WithMany(rm => rm.Reservations).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>().HasOne(r => r.User)
                .WithMany(s => s.Reservations).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>().HasOne(r => r.Guest)
                 .WithMany(s => s.Reservations).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserService>().HasOne(r => r.User)
               .WithMany(s => s.UserService).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserService>().HasOne(r => r.Service)
                .WithMany(s => s.UserService).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>().HasOne(r => r.User)
                .WithMany(rm => rm.Room).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>().HasIndex(r => r.RoomNumber).IsUnique();

        }
        public DbSet<User> User { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<UserService> UserServices { get; set; }
        }
}
