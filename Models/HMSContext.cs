using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HMS.Models
{
    public class HMSContext : IdentityDbContext<Guest>
    {
        public HMSContext(DbContextOptions<HMSContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>().HasOne(r => r.Guest)
                .WithMany(g => g.Reservations).HasForeignKey(r => r.GuestId);

            modelBuilder.Entity<Reservation>().HasOne(r => r.Room)
                .WithMany(rm => rm.Reservations).HasForeignKey(r => r.RoomId);

            modelBuilder.Entity<Reservation>().HasOne(r => r.Staff)
                .WithMany(s => s.Reservations).HasForeignKey(r => r.StaffId);

            modelBuilder.Entity<StaffService>().HasOne(r => r.Staff)
                .WithMany(s => s.StaffService).HasForeignKey(r => r.StaffId);

            modelBuilder.Entity<StaffService>().HasOne(r => r.Service)
                .WithMany(s => s.StaffService).HasForeignKey(r => r.ServiceId);

            modelBuilder.Entity<GuestService>().HasOne(r => r.Guest)
               .WithMany(s => s.GuestService).HasForeignKey(r => r.GuestId);

            modelBuilder.Entity<GuestService>().HasOne(r => r.Service)
                .WithMany(s => s.GuestService).HasForeignKey(r => r.ServiceId);

        }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
