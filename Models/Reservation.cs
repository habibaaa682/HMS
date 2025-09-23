namespace HMS.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int RoomId { get; set; }
        public string GuestId { get; set; }
        public int StaffId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Room Room { get; set; }
        public Guest Guest { get; set; }
        public Staff Staff { get; set; }
    }
}
