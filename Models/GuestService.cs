namespace HMS.Models
{
    public class GuestService
    {
        public int GuestServiceId { get; set; }
        public string GuestId { get; set; }
        public Guest? Guest { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
