using HMS.Models;
using HMS.Models.DTO;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomController : BaseBusinessController<Room ,RoomDto , IRoomServices>
    {
        private readonly IRoomServices _roomServices;
        public RoomController(IRoomServices roomServices)
        {
            _roomServices = roomServices;
        }

        [HttpPost]
        public override async Task<IActionResult> Insert(RoomDto room)
        {
            var createdRoom = await _roomServices.Insert(room, UserId);
            if(createdRoom == null) return BadRequest("Room not created");
            return Ok(createdRoom);
        } 
        
        [HttpPut]
        public override async Task<IActionResult> Edit(RoomDto room)
        {
            var updatedRoom = await _roomServices.EditRoom(room, UserId);
            if (updatedRoom == null) return BadRequest("Room not updated");
            return Ok(updatedRoom);
        }

        [HttpDelete]
        public override async Task<IActionResult> Remove(int roomId)
        {
            var deletedRoom = await _roomServices.Remove(roomId, UserId);
            if (deletedRoom == false) return BadRequest("Room not deleted");
            return Ok(deletedRoom);
        }

        [HttpGet("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomServices.GetAllRooms();
            if (rooms == null) return NotFound("No rooms found");
            return Ok(rooms);
        }

        [HttpGet("GetRoomById/{roomId}")]
        public async Task<IActionResult> GetRoomById(int roomId)
        {
            var room = await _roomServices.GetRoomById(roomId);
            if (room == null) return NotFound("Room not found");
            return Ok(room);
        }

    }
}
