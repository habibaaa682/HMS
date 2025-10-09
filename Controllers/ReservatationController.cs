using HMS.Models;
using HMS.Models.DTO;
using HMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
    public class ReservatationController : BaseBusinessController<Reservation, ReservationDto, IReservationServices>
    {
        private readonly IReservationServices _reservationServices;
        public ReservatationController(IReservationServices reservationServices)
        {
            _reservationServices = reservationServices;
        }

        [HttpPost]
        public override async Task<IActionResult> Insert(ReservationDto reservation)
        {
            var createdReservation = await _reservationServices.Insert(reservation, UserId);
            if (createdReservation == null) return BadRequest("Reservation not created");
            return Ok(createdReservation);
        }

        [HttpPut]
        public override async Task<IActionResult> Edit(ReservationDto reservation)
        {
            var updatedReservation = await _reservationServices.Edit(reservation, UserId);
            if (updatedReservation == null) return BadRequest("Reservation not updated");
            return Ok(updatedReservation);
        }

        [HttpDelete]
        public override async Task<IActionResult> Remove(int reservationId)
        {
            var deletedReservation = await _reservationServices.Remove(reservationId, UserId);
            if (deletedReservation == false) return BadRequest("Reservation not deleted");
            return Ok(deletedReservation);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _reservationServices.GetAllReservations();
            if (reservations == null) return NotFound("No Reservation found");
            return Ok(reservations);
        }

        [HttpGet]
        public async Task<IActionResult> GetReservationById(int reservationId)
        {
            var reservation = await _reservationServices.GetReservationById(reservationId);
            if (reservation == null) return NotFound("Reservation not found");
            return Ok(reservation);
        }
    }
}
