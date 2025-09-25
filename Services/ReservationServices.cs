using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;
using HMS.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public interface IReservationServices
    {
        Task<object> AddReservation(ReservationDto reservationDto, string id);
        Task<object> EditReservation(ReservationDto reservationDto, string id);
        Task<bool> RemoveReservation(int reservationId, string id);
        Task<object> GetAllReservations();
        Task<object> GetReservationById(int resrvationId);
    }

    public class ReservationServices : IReservationServices
    {
        private readonly HMSContext _context;

        private readonly IMapper _mapper;
        private readonly HMSContext _db;
        public ReservationServices(IMapper mapper, HMSContext db)
        {
            _mapper = mapper;
            _db = db;

        }
        public async Task<object> AddReservation(ReservationDto reservationDto, string id)
        {
            var user = await _db.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");
            if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
                throw new Exception("Only Admin or Staff can Add Reservation");
            Reservation resrvation = _mapper.Map<Reservation>(reservationDto);
            resrvation.UserId = id;
            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == reservationDto.RoomId);
            if (room == null) throw new Exception("Room not found");
            if (!room.IsAvailable) throw new Exception("Room is not available");
            room.IsAvailable = false;
            await _db.Reservations.AddAsync(resrvation);
            await _db.SaveChangesAsync();
            return _mapper.Map<Reservation>(reservationDto);
        }
        public async Task<object> EditReservation(ReservationDto reservationDto, string id)
        {
            var user = await _db.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");

            if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
                throw new Exception("Only Admin or Staff can update reservations");

            var reservation = await _db.Reservations
                .FirstOrDefaultAsync(r => r.ReservationId == reservationDto.ReservationId);

            if (reservation == null) throw new Exception("Reservation not found");

            if (reservationDto.RoomId > 0)
                reservation.RoomId = reservationDto.RoomId;

            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == reservationDto.RoomId);
            if (room == null) throw new Exception("Room not found");
            if (!room.IsAvailable) throw new Exception("Room is not available");
            room.IsAvailable = false;

            if (!string.IsNullOrEmpty(reservationDto.UserId))
                reservation.UserId = reservationDto.UserId;

            if (reservationDto.CheckInDate.HasValue)
                reservation.CheckInDate = reservationDto.CheckInDate.Value;

            if (reservationDto.CheckOutDate.HasValue)
                reservation.CheckOutDate = reservationDto.CheckOutDate.Value;

            if (reservationDto.TotalPrice > 0)
                reservation.TotalPrice = reservationDto.TotalPrice;

            await _db.SaveChangesAsync();

            return reservationDto;
        }

        public async Task<bool> RemoveReservation(int reservationId, string id)
        {
            var user = await _db.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");
            if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
                throw new Exception("Only Admin or Staff can Delete Reservation");
            var reservation = await _db.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
            if (reservation == null) throw new Exception("Room not found");
            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<object> GetAllReservations()
        {
            var resrvations = await _db.Reservations.Select(s => new
            {
                s.RoomId,
                s.CheckInDate,
                s.CheckOutDate,
                s.TotalPrice,
                s.UserId,
                user = new
                {
                    s.User.Id,
                    s.User.UserName,
                    s.User.Email,
                    s.User.UserType
                }
            }).ToListAsync();
            if (resrvations == null) throw new Exception("No rooms found");
            return resrvations;
        }
        public async Task<object> GetReservationById(int reservationId)
        {
            var resrvation = await _db.Reservations.Select(s => new
            {
                s.ReservationId,
                s.RoomId,
                s.CheckInDate,
                s.CheckOutDate,
                s.TotalPrice,
                s.UserId,
                user = new
                {
                    s.User.Id,
                    s.User.UserName,
                    s.User.Email,
                    s.User.UserType
                }
            }).FirstOrDefaultAsync(r => r.ReservationId == reservationId);
            if (resrvation == null) throw new Exception("resrvation not found");
            return resrvation;
        }
    }

}

