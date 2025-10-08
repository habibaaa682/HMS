namespace HMS.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public required string ServiceName { get; set; }
        public decimal Price { get; set; }
        public ICollection<UserService>? UserServices { get; set; }
        public ICollection<StaffService>? StaffServices { get; set; }
    }
}
