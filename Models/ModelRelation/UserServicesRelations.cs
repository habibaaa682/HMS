using Microsoft.EntityFrameworkCore;

namespace HMS.Models.ModelRelation
{
    public static class UserServicesRelations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserService>().HasOne(us => us.User)
                .WithMany(u => u.UserServices).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserService>().HasOne(us => us.Service)
                .WithMany(s => s.UserServices).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
