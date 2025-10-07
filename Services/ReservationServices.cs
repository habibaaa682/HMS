using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;
using HMS.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public interface IReservationServices: IBaseBusinessService<Reservation, ReservationDto>
    {
        Task<object> AddReservation(ReservationDto reservationDto, string id);
        Task<object> EditReservation(ReservationDto reservationDto, string id);
        Task<bool> RemoveReservation(int reservationId, string id);
        Task<object> GetAllReservations();
        Task<object> GetReservationById(int resrvationId);
    }

    public class ReservationServices(HMSContext db, IMapper mapper) : BaseBusinessService<Reservation, ReservationDto>(mapper, db), IReservationServices
    {

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

            if (reservationDto.RoomId > 0 && reservationDto.RoomId != reservation.RoomId)
            {
                var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == reservationDto.RoomId);
                if (room == null) throw new Exception("Room not found");
                if (!room.IsAvailable) throw new Exception("Room is not available");

                var oldRoom = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == reservation.RoomId);
                if (oldRoom != null) oldRoom.IsAvailable = true;

                reservation.RoomId = reservationDto.RoomId;
                room.IsAvailable = false;
            }
            if (reservationDto.CheckInDate.HasValue)
                reservation.CheckInDate = reservationDto.CheckInDate.Value;

            if (reservationDto.CheckOutDate.HasValue)
                reservation.CheckOutDate = reservationDto.CheckOutDate.Value;

            if (reservationDto.TotalPrice > 0)
                reservation.TotalPrice = reservationDto.TotalPrice;

            await _db.SaveChangesAsync();
            var obj = await GetReservationById(reservationDto.ReservationId);
            return obj;
        }

        public async Task<bool> RemoveReservation(int reservationId, string id)
        {
            var user = await _db.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");
            if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
                throw new Exception("Only Admin or Staff can Delete Reservation");
            var reservation = await _db.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
            if (reservation == null) throw new Exception("Reservation not found");
            var Room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == reservation.RoomId);
            if (Room != null) Room.IsAvailable = true;
            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<object> GetAllReservations()
        {
            var resrvations = await _db.Reservations.Select(s => new
            {
                s.ReservationId,
                Room = new
                {
                    s.Room!.RoomId,
                    s.Room.RoomNumber,
                    s.Room.RoomType,
                    s.Room.PricePerNight,
                    s.Room.IsAvailable,
                },
                s.CheckInDate,
                s.CheckOutDate,
                s.TotalPrice,
                s.UserId,
                user = new
                {
                    s.User!.Id,
                    s.User.UserName,
                    s.User.Email,
                    s.User.UserType
                },
                Guests = new
                {
                    s.Guest!.GuestId,
                    s.Guest.FirstName,
                    s.Guest.LastName,
                    s.Guest.Email,
                    s.Guest.PhoneNumber,
                    s.Guest.Address
                }
            }).ToListAsync();
            if (resrvations == null) throw new Exception("No Reservations found");
            return resrvations;
        }
        public async Task<object> GetReservationById(int reservationId)
        {
            var resrvation = await _db.Reservations.Select(s => new
            {
                s.ReservationId,
                Room = new
                {
                    s.Room!.RoomId,
                    s.Room.RoomNumber,
                    s.Room.RoomType,
                    s.Room.PricePerNight,
                    s.Room.IsAvailable,
                },
                s.CheckInDate,
                s.CheckOutDate,
                s.TotalPrice,
                s.UserId,
                user = new
                {
                    s.User!.Id,
                    s.User.UserName,
                    s.User.Email,
                    s.User.UserType
                },
                Guests = new
                {
                    s.Guest!.GuestId,
                    s.Guest.FirstName,
                    s.Guest.LastName,
                    s.Guest.Email,
                    s.Guest.PhoneNumber,
                    s.Guest.Address
                }
            }).FirstOrDefaultAsync(r => r.ReservationId == reservationId);
            if (resrvation == null) throw new Exception("resrvation not found");
            return resrvation;
        }
    }

}

