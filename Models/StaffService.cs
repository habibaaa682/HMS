namespace HMS.Models
{
    public class StaffService
    {
        public int StaffServiceId { get; set; }
        public int StaffId { get; set; }
        public Staff? Staff { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
