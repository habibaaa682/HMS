using Microsoft.EntityFrameworkCore;

namespace HMS.Models.ModelRelation
{
    public static class ServiceRelation
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>().HasIndex(s => s.ServiceName).IsUnique();
            modelBuilder.Entity<Service>().HasMany(s => s.UserServices).WithOne(u => u.Service).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Service>().HasMany(s => s.StaffServices).WithOne(u => u.Service).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Service>().HasMany(s => s.GuestServices).WithOne(u => u.Service).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
