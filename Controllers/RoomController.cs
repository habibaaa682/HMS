using HMS.Models.DTO;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomController : ControllerBase
    {
        public string UserId { get { return GetUserId(); } }
        private string GetUserId()
        {
            string? m = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(m)) return m;
            throw new Exception("Error In Read UserId From Token");
        }
        private readonly IRoomServices _roomServices;
        public RoomController(IRoomServices roomServices)
        {
            _roomServices = roomServices;
        }

        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateRoom(RoomDto room)
        {
            var createdRoom = await _roomServices.Insert(room, UserId);
            if(createdRoom == null) return BadRequest("Room not created");
            return Ok(createdRoom);
        } 
        
        [HttpPut("UpdateRoom")]
        public async Task<IActionResult> UpdateRoom(RoomDto room)
        {
            var updatedRoom = await _roomServices.EditRoom(room, UserId);
            if (updatedRoom == null) return BadRequest("Room not updated");
            return Ok(updatedRoom);
        }

        [HttpDelete("DeleteRoom")]
        public async Task<IActionResult> DeleteRoom(int roomId)
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
