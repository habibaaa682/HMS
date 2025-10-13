using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;

namespace HMS.MiddleWares
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<RoomDto, Room>().ReverseMap();
            CreateMap<ReservationDto, Reservation>().ReverseMap();
            CreateMap<GuestDto, Guest>().ReverseMap();
            CreateMap<StaffDto, Staff>().ReverseMap();
            CreateMap<ServiceDto, Service>().ReverseMap();
        }
    }
}
