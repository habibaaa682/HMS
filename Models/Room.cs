namespace HMS.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public required string RoomNumber { get; set; }
        public required string RoomType { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
