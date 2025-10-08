using HMS.Models.DTO;
using HMS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        public string UserId { get { return GetUserId(); } }
        private string GetUserId()
        {
            string? m = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(m)) return m;
            throw new Exception("Error In Read UserId From Token");
        }
        private readonly IGuestServices _guestServices;
        public GuestController(IGuestServices guestServices)
        {
            _guestServices = guestServices;
        }
        [HttpPost("CreateGuest")]
        public async Task<IActionResult> CreateGuest(GuestDto guest)
        {
            var createdGuest = await _guestServices.Insert(guest, UserId);
            if (createdGuest == null) return BadRequest("Guest not created");
            return Ok(createdGuest);
        }
        [HttpPut("UpdateGuest")]
        public async Task<IActionResult> EditGuest(GuestDto guest)
        {
            var updatedGuest = await _guestServices.Edit(guest, UserId);
            if (updatedGuest == null) return BadRequest("Guest not updated");
            return Ok(updatedGuest);
        }
        [HttpDelete("DeleteGuest")]
        public async Task<IActionResult> DeleteGuest(int guestId)
        {
            var deletedGuest = await _guestServices.Remove(guestId, UserId);
            if (deletedGuest == false) return BadRequest("Guest not deleted");
            return Ok(deletedGuest);
        }
        [HttpGet("GetAllGuests")]
        public async Task<IActionResult> GetAllGuests()
        {
            var guests = await _guestServices.GetAllGuests();
            if (guests == null) return NotFound("No Guest found");
            return Ok(guests);
        }
        [HttpGet("GetGuestById/{guestId}")]
        public async Task<IActionResult> GetGuestById(int guestId)
        {
            var guest = await _guestServices.GetGuestById(guestId);
            if (guest == null) return NotFound("Guest not found");
            return Ok(guest);
        }
    }
}
