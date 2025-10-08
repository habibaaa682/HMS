using HMS.Models.Enum;

namespace HMS.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        public string? UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public required StaffPostionTypeEnum Postion { get; set; }
        public ICollection<StaffService>? StaffServices { get; set; }
        public User? User { get; set; }
    }
}
