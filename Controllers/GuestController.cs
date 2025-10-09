using HMS.Models;
using HMS.Models.DTO;
using HMS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : BaseBusinessController<Guest, GuestDto, IGuestServices>
    {
        private readonly IGuestServices _guestServices;
        public GuestController(IGuestServices guestServices)
        {
            _guestServices = guestServices;
        }
        [HttpPost]
        public override async Task<IActionResult> Insert(GuestDto guest)
        {
            var createdGuest = await _guestServices.Insert(guest, UserId);
            if (createdGuest == null) return BadRequest("Guest not created");
            return Ok(createdGuest);
        }
        [HttpPut]
        public override async Task<IActionResult> Edit(GuestDto guest)
        {
            var updatedGuest = await _guestServices.Edit(guest, UserId);
            if (updatedGuest == null) return BadRequest("Guest not updated");
            return Ok(updatedGuest);
        }
        [HttpDelete]
        public override async Task<IActionResult> Remove(int guestId)
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
