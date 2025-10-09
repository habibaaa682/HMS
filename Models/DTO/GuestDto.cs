namespace HMS.Models.DTO
{
    public class GuestDto
    {
        public  int? GuestId { get; set; }
        public string UserId { get; set; } = null!;
        public  string? FirstName { get; set; }
        public  string? LastName { get; set; }
        public  string? Email { get; set; }
        public  string? PhoneNumber { get; set; }
        public  string? Address { get; set; }
    }
}
