﻿namespace HMS.Models
{
    public class Guest
    {
        public int GuestId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public ICollection<Reservation> ?Reservations { get; set; }

    }
}
