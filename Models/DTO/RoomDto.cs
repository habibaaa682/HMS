namespace HMS.Models.DTO
{
    public class RoomDto
    {
        public int RoomId { get; set; }
        public string UserId { get; set; } = null!;

        public string? RoomNumber { get; set; }
        public  string? RoomType { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
    }
}
