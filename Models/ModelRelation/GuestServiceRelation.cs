using Microsoft.EntityFrameworkCore;

namespace HMS.Models.ModelRelation
{
    public static class GuestServiceRelation
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuestService>().HasOne(gs => gs.Guest).WithMany(g => g.GuestServices).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GuestService>().HasOne(gs => gs.Service).WithMany(s => s.GuestServices).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
