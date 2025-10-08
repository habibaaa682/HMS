using Microsoft.EntityFrameworkCore;

namespace HMS.Models.ModelRelation
{
    public static class GuestRelation
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>().HasOne(g => g.User).WithOne(u => u.Guest).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Guest>().HasMany(g => g.Reservations).WithOne(r => r.Guest).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
