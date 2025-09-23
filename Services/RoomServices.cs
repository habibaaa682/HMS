using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;
using HMS.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public interface IRoomServices
    {
        Task<string> AddRoom(RoomDto roomDto,string id);
    }
    public class RoomServices: IRoomServices
    {
        private readonly IMapper _mapper;
        private readonly HMSContext _db;
        public RoomServices(IMapper mapper,HMSContext db)
        {
            _mapper = mapper;
            _db = db;
        }
        public async Task<string> AddRoom(RoomDto roomDto,string id)
        {
            var user= await _db.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");
            if (user.UserType != UserTypeEnum.Admin) throw new Exception("Only Admin can add rooms");
            var room = _mapper.Map<Room>(roomDto);
            await _db.Rooms.AddAsync(room);
            await _db.SaveChangesAsync();
            return "Room Added";
        }
    }
}
