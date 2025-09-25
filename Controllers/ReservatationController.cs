using HMS.Models.DTO;
using HMS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservatationController : ControllerBase
    {
        public string UserId { get { return GetUserId(); } }
        private string GetUserId()
        {
            string? m = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(m)) return m;
            throw new Exception("Error In Read UserId From Token");
        }
        private readonly IReservationServices _reservationServices;
        public ReservatationController(IReservationServices reservationServices)
        {
            _reservationServices = reservationServices;
        }

        [HttpPost("CreateReservation")]
        public async Task<IActionResult> CreateReservation(ReservationDto reservation)
        {
            var createdReservation = await _reservationServices.AddReservation(reservation, UserId);
            if (createdReservation == null) return BadRequest("Reservation not created");
            return Ok(createdReservation);
        }

        [HttpPut("UpdateReservation")]
        public async Task<IActionResult> EditReservation(ReservationDto reservation)
        {
            var updatedReservation = await _reservationServices.EditReservation(reservation, UserId);
            if (updatedReservation == null) return BadRequest("Reservation not updated");
            return Ok(updatedReservation);
        }

        [HttpDelete("DeleteReservation")]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            var deletedReservation = await _reservationServices.RemoveReservation(reservationId, UserId);
            if (deletedReservation == false) return BadRequest("Reservation not deleted");
            return Ok(deletedReservation);
        }

        [HttpGet("GetAllReservation")]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _reservationServices.GetAllReservations();
            if (reservations == null) return NotFound("No Reservation found");
            return Ok(reservations);
        }

        [HttpGet("GetResrvationById/{reservationId}")]
        public async Task<IActionResult> GetReservationById(int reservationId)
        {
            var reservation = await _reservationServices.GetReservationById(reservationId);
            if (reservation == null) return NotFound("Reservation not found");
            return Ok(reservation);
        }
    }
}
