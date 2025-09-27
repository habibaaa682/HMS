using Microsoft.EntityFrameworkCore;

namespace HMS.Models.ModelRelation
{
    public static class RoomRelations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasIndex(r => r.RoomNumber).IsUnique();
            modelBuilder.Entity<Room>().HasOne(r => r.User)
                .WithMany(u => u.Room).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
