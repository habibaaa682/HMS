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
            ModelRelation.RoomRelations.Configure(modelBuilder);
            ModelRelation.ReservationRelations.Configure(modelBuilder);
            ModelRelation.UserServicesRelations.Configure(modelBuilder);
        }
        public DbSet<User> User { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<UserService> UserServices { get; set; }
        }
}
