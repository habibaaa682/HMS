namespace HMS.Models.DTO
{
    public class RoomDto
    {
        public int RoomId { get; set; }
        public required string RoomNumber { get; set; }
        public required string RoomType { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
    }
}
