using Microsoft.EntityFrameworkCore;

namespace HMS.Models
{
    public class HMSContext : DbContext
    {
        public HMSContext(DbContextOptions<HMSContext> options) : base(options)
        {
        }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}
