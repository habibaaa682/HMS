using Microsoft.EntityFrameworkCore;

namespace HMS.Models.ModelRelation
{
    public static class StaffServiceRelation
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffService>().HasOne(us => us.Staff).WithMany(s => s.StaffServices).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StaffService>().HasOne(us => us.Service).WithMany(s => s.StaffServices).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
