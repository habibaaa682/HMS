using HMS.Models;
using HMS.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Guest> _userManager;
        public AccountController(UserManager<Guest> userManager) {
            _userManager = userManager;
        }
    
    [HttpPost]
        public async Task<IActionResult> Register(GuestDto guestDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var guest = new Guest {
                FirstName = guestDto.FirstName,
                LastName = guestDto.LastName,
                UserName = guestDto.UserName,
                Email = guestDto.Email,
                PhoneNumber = guestDto.PhoneNumber
            };
            var result = await _userManager.CreateAsync(guest, guestDto.Password);
            if (result.Succeeded) {
                return Ok("User registered successfully");
            }
            return BadRequest(result.Errors);
        }
    }
}
