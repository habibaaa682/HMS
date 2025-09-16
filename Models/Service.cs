namespace HMS.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public required string ServiceName { get; set; }
        public decimal Price { get; set; }
        public ICollection<Staff>? StaffMembers { get; set; }
        public ICollection<StaffService>? StaffService { get; set; }
        public ICollection<GuestService>? GuestService { get; set; }
    }
}
