using Microsoft.EntityFrameworkCore;

namespace HMS.Models.ModelRelation
{
    public static class ReservationRelations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>().HasOne(r => r.Room)
                .WithMany(rm => rm.Reservations).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Reservation>().HasOne(r => r.User)
                .WithMany(s => s.Reservations).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Reservation>().HasOne(r => r.Guest)
                 .WithMany(s => s.Reservations).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
