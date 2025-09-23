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
                .WithMany(rm => rm.Reservations).HasForeignKey(r => r.RoomId);

            modelBuilder.Entity<Reservation>().HasOne(r => r.User)
                .WithMany(s => s.Reservations).HasForeignKey(r => r.UserId);

            modelBuilder.Entity<UserService>().HasOne(r => r.User)
               .WithMany(s => s.UserService).HasForeignKey(r => r.UserId);

            modelBuilder.Entity<UserService>().HasOne(r => r.Service)
                .WithMany(s => s.UserService).HasForeignKey(r => r.ServiceId);

        }
        public DbSet<User> User { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<UserService> UserServices { get; set; }

        }
}
