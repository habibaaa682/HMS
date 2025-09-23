namespace HMS.Models
{
    public class UserService
    {
        public int UserServiceId { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
