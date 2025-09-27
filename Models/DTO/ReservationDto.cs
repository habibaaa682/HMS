namespace HMS.Models.DTO
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public DateTime ?CheckInDate { get; set; }
        public DateTime ?CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
