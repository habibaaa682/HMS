using HMS.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.DTO
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public UserTypeEnum UserType { get; set; }

    }
}
