using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;
using HMS.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public interface IGuestServices: IBaseBusinessService<Guest, GuestDto>
    {
        Task<object> GetAllGuests();
        Task<object> GetGuestById(int guestId);
    }
    public class GuestServices(HMSContext db, IMapper mapper) : BaseBusinessService<Guest, GuestDto>(mapper, db), IGuestServices
    {
        public override async Task<GuestDto?> Insert(GuestDto guestDto, string id)
        {
            try { 
            var user = await db.Users.FirstOrDefaultAsync(s => s.Id == id);

            if (user == null) throw new Exception("User not found");
            if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
            throw new Exception("Only Admin or Staff can Add Reservation");
                guestDto.UserId = id;
                var result = await base.Insert(guestDto, id);
            return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override async Task<GuestDto?> Edit(GuestDto guestDto, string id)
        {
            try { 
            var user = await db.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");

            if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
                throw new Exception("Only Admin or Staff can update reservations");

            var guest = await db.Guests
                .FirstOrDefaultAsync(r => r.GuestId == guestDto.GuestId);
            if (guest == null) throw new Exception("guest not found");
            var updated = await base.Edit(guestDto, id);

            return updated;
        }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task<object> GetAllGuests()
        {
            var guests = await db.Guests.Select(g => new
            {
                g.GuestId,
                g.FirstName,
                g.LastName,
                g.Email,
                g.PhoneNumber,
                g.Address,
                user = new
                {
                    g.User!.Id,
                    g.User.UserName,
                    g.User.Email,
                    g.User.UserType
                }
                })
                .ToListAsync();
            return guests;

        }

        public async Task<object> GetGuestById(int guestId)
        {
            var guest = await db.Guests
                .Where(g => g.GuestId == guestId)
                .Select(g => new
                {
                    g.GuestId,
                    g.FirstName,
                    g.LastName,
                    g.Email,
                    g.PhoneNumber,
                    g.Address,
                    user = new
                    {
                        g.User!.Id,
                        g.User.UserName,
                        g.User.Email,
                        g.User.UserType
                    }
                })
                .FirstOrDefaultAsync();
            return guest!;

        }
        public override async Task<bool> Remove(int GuestId, string id)
        {
            try
            {
                var user = await db.User.FirstOrDefaultAsync(s => s.Id == id);
                if (user == null) throw new Exception("User not found");
                if (user.UserType != UserTypeEnum.Admin) throw new Exception("Only Admin can delete guests");
                var guest = db.Guests.FirstOrDefault(g => g.GuestId == GuestId);
                if (guest == null) throw new Exception("Guest not found");
                var result = await base.Remove(GuestId, id);
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }
    }
}