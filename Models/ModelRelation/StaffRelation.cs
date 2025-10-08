using Microsoft.EntityFrameworkCore;

namespace HMS.Models.ModelRelation
{
    public static class StaffRelation
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasOne(u => u.Staff).WithOne(s => s.User).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Staff>().HasMany(s => s.StaffServices).WithOne(ss => ss.Staff).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
