using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;

namespace HMS.MiddleWares
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<RoomDto, Room>();
        }
    }
}
