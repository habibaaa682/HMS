using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;
using HMS.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public interface IGuestServices
    {
        Task<object> GetAllGuests();
        Task<object> GetGuestById(int guestId);
        Task<object> AddGuest(GuestDto guestDto, string id);
        Task<object> EditGuest(GuestDto guestDto, string id);
        Task<bool> RemoveGuest(int GuestId, string id);
    }
    public class GuestServices : IGuestServices
    {
        private readonly HMSContext _context;
        private readonly IMapper _mapper;
        public GuestServices(IMapper mapper, HMSContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<object> AddGuest(GuestDto guestDto, string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.Id == id);

            if (user == null) throw new Exception("User not found");
            if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
            throw new Exception("Only Admin or Staff can Add Reservation");
            var guest = _mapper.Map<Guest>(guestDto);
            guest.UserId = id;
            await _context.Guests.AddAsync(guest);
            await _context.SaveChangesAsync();
            return _mapper.Map<Guest>(guestDto);
        }

        public async Task<object> EditGuest(GuestDto guestDto, string id)
        {
            var user = await _context.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");

            if (user.UserType != UserTypeEnum.Admin && user.UserType != UserTypeEnum.Staff)
                throw new Exception("Only Admin or Staff can update reservations");

            var guest = await _context.Guests
                .FirstOrDefaultAsync(r => r.GuestId == guestDto.GuestId);
            if (guest == null) throw new Exception("guest not found");
            if (!string.IsNullOrEmpty(guestDto.FirstName))
                guest.FirstName = guestDto.FirstName;
            if (!string.IsNullOrEmpty(guestDto.LastName))
                guest.LastName = guestDto.LastName;
            if (!string.IsNullOrEmpty(guestDto.Email))
                guest.Email = guestDto.Email;
            if (!string.IsNullOrEmpty(guestDto.PhoneNumber))
                guest.PhoneNumber = guestDto.PhoneNumber;
            if (!string.IsNullOrEmpty(guestDto.Address))
                guest.Address = guestDto.Address;
                await _context.SaveChangesAsync();
            var obj = await GetGuestById(guestDto.GuestId);
            return obj;
        }

        public async Task<object> GetAllGuests()
        {
            var guests = await _context.Guests.Select(g => new
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
            var guest = await _context.Guests
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
        public async Task<bool> RemoveGuest(int GuestId, string id)
        {
            var user = await _context.User.FirstOrDefaultAsync(s => s.Id == id);
            if (user == null) throw new Exception("User not found");
            if (user.UserType != UserTypeEnum.Admin) throw new Exception("Only Admin can delete guests");
            var guest = _context.Guests.FirstOrDefault(g => g.GuestId == GuestId);
            if (guest == null) throw new Exception("Guest not found");
            _context.Guests.Remove(guest);
            _context.SaveChanges();
            return true;

        }
    }
}