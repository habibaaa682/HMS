namespace HMS.Models.DTO
{
    public class ServiceDto
    {
        public int ServiceId { get; set; }
        public string? UserId { get; set; }
        public required string ServiceName { get; set; }
        public decimal Price { get; set; }
    }
}
