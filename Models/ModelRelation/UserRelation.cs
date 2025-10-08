using Microsoft.EntityFrameworkCore;

namespace HMS.Models.ModelRelation
{
    public static class UserRelation
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasOne(u => u.Staff).WithOne(s => s.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(u => u.Room).WithOne(r => r.User).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasMany(u => u.Reservations).WithOne(r => r.User).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasMany(u => u.UserServices).WithOne(r => r.User).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
