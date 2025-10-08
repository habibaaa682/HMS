using HMS.Models.Enum;

namespace HMS.Models.DTO
{
    public class StaffDto
    {
        public int? StaffId { get; set; }
        public string? UserId { get; set; }
        public  string? FirstName { get; set; }
        public  string? LastName { get; set; }
        public  string? Email { get; set; }
        public  string? PhoneNumber { get; set; }
        public  string? Address { get; set; }
        public  StaffPostionTypeEnum Postion { get; set; }
    }
}
