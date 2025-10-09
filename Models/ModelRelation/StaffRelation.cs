using Microsoft.EntityFrameworkCore;

namespace HMS.Models.ModelRelation
{
    public static class StaffRelation
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>().HasMany(s => s.StaffServices).WithOne(ss => ss.Staff).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Staff>().HasOne(s => s.User).WithMany(u => u.Staff).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
