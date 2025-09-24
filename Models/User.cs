using HMS.Models.Enum;
using Microsoft.AspNetCore.Identity;

namespace HMS.Models
{
    public class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required UserTypeEnum UserType { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<UserService>? UserService { get; set; }
        public ICollection<Room>? Room { get; set; }

    }
}
