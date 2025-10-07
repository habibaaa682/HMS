using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;
using HMS.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public interface IReservationServices: IBaseBusinessService<Reservation, ReservationDto>
    {
        Task<object> GetAllReservations();
        Task<object> GetReservationById(int resrvationId);
    }

    public class ReservationServices(HMSContext db, IMapper mapper) : BaseBusinessService<Reservation, ReservationDto>(mapper, db), IReservationServices
    {

        public override async Task<ReservationDto?> Insert(ReservationDto reservationDto, string userId)
        {
            try
            {
                var user = await _db.User.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    throw new Exception("User not found");
                if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
                    throw new Exception("Only Admin or Staff can add a reservation");
                var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == reservationDto.RoomId);
                if (room == null)
                    throw new Exception("Room not found");
                if (!room.IsAvailable)
                    throw new Exception("Room is not available");
                room.IsAvailable = false;
                var result = await base.Insert(reservationDto, userId);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting reservation: {ex.Message}");
                return null;
            }
        }

        public override async Task<ReservationDto?> Edit(ReservationDto reservationDto, string userId)
        {
            try
            {
                var user = await _db.User.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    throw new Exception("User not found");

                if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
                    throw new Exception("Only Admin or Staff can update reservations");

                var existingReservation = await _db.Reservations
                    .FirstOrDefaultAsync(r => r.ReservationId == reservationDto.ReservationId);

                if (existingReservation == null)
                    throw new Exception("Reservation not found");

                if (reservationDto.RoomId > 0 && reservationDto.RoomId != existingReservation.RoomId)
                {
                    var newRoom = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == reservationDto.RoomId);
                    if (newRoom == null)
                        throw new Exception("Room not found");
                    if (!newRoom.IsAvailable)
                        throw new Exception("Room is not available");

                    var oldRoom = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == existingReservation.RoomId);
                    if (oldRoom != null)
                        oldRoom.IsAvailable = true;

                    newRoom.IsAvailable = false;
                }
                var updated = await base.Edit(reservationDto, userId);

                return updated;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error editing reservation: {ex.Message}");
                return null;
            }
        }


        public override async Task<bool> Remove(int reservationId, string userId)
        {
            try
            {
                var user = await _db.User.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    throw new Exception("User not found");

                if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
                    throw new Exception("Only Admin or Staff can delete a reservation");

                var reservation = await _db.Reservations
                    .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

                if (reservation == null)
                    throw new Exception("Reservation not found");

                var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == reservation.RoomId);
                if (room != null)
                    room.IsAvailable = true;

                var result = await base.Remove(reservationId,userId);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing reservation: {ex.Message}");
                return false;
            }
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

