using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;
using HMS.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public interface IRoomServices: IBaseBusinessService<Room, RoomDto>
    {
        Task<object> EditRoom(RoomDto roomDto,string id);
        Task<object> GetAllRooms();
        Task<object> GetRoomById(int roomId);
    }
    public class RoomServices(HMSContext db, IMapper mapper): BaseBusinessService<Room, RoomDto>( mapper, db), IRoomServices
    {

        public override async Task<RoomDto?> Insert(RoomDto roomDto, string id)
        {
            try { 
            var user = await _db.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");
            if (user.UserType != UserTypeEnum.Admin) throw new Exception("Only Admin can add rooms");
            roomDto.UserId = id;
                var result = await base.Insert(roomDto, id);
            return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<object> EditRoom(RoomDto roomDto, string id)
        {
            var user = await _db.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");
            if (user.UserType != UserTypeEnum.Admin) throw new Exception("Only Admin can update rooms");
            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomDto.RoomId);
            if (room == null) throw new Exception("Room not found");

            if (!string.IsNullOrEmpty(roomDto.RoomNumber))
                room.RoomNumber = roomDto.RoomNumber;

            if (!string.IsNullOrEmpty(roomDto.RoomType))
                room.RoomType = roomDto.RoomType;

            if (roomDto.PricePerNight > 0)
                room.PricePerNight = roomDto.PricePerNight;

            room.IsAvailable = roomDto.IsAvailable;
            await _db.SaveChangesAsync();
            var obj = await GetRoomById(roomDto.RoomId);
            return obj;
        }
        public override async Task<bool> Remove(int roomId, string id)
        {
            try { 
            var user = await _db.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");
            if (user.UserType != UserTypeEnum.Admin) throw new Exception("Only Admin can update rooms");
            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
            if (room == null) throw new Exception("Room not found");
            var result = await base.Remove(roomId, id);
            return true;
        }
            catch (Exception)
            {
                return false;
            }
}
        public async Task<object> GetAllRooms()
        {
            var rooms = await _db.Rooms.Select(s => new
            {
                s.RoomId,
                s.RoomNumber,
                s.RoomType,
                s.PricePerNight,
                s.IsAvailable,
                s.UserId,
                user = new
                {
                    s.User.Id,
                    s.User.UserName,
                    s.User.Email,
                    s.User.UserType
                }
            }).ToListAsync();
            if (rooms == null) throw new Exception("No rooms found");
            return rooms;
        }
        public async Task<object> GetRoomById(int roomId)
        {
            var room = await _db.Rooms.Select(s => new
            {
                s.RoomId,
                s.RoomNumber,
                s.RoomType,
                s.PricePerNight,
                s.IsAvailable,
                s.UserId,
                user = new
                {
                    s.User.Id,
                    s.User.UserName,
                    s.User.Email,
                    s.User.UserType
                }
            }).FirstOrDefaultAsync(r => r.RoomId == roomId);
            if (room == null) throw new Exception("Room not found");
            return room;
        }
    }
}
