using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;
using HMS.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public interface IStaffServices : IBaseBusinessService<Staff, StaffDto>
    {
        Task<object> GetStaffById(int staffId);
        Task<object> GetAllStaffs();
    }
    public class StaffServices(HMSContext db, IMapper mapper) : BaseBusinessService<Staff, StaffDto>(mapper, db), IStaffServices
    {
        public override async Task<StaffDto?> Insert(StaffDto staffDto, string id)
        {
            try
            {
                var user = await _db.User.FirstOrDefaultAsync(s => s.Id == id);
                if (user == null) throw new Exception("User not found");
                if (user.UserType != UserTypeEnum.Admin) throw new Exception("Only Admin can add Staff");
                staffDto.UserId = id;
                var result = await base.Insert(staffDto, id);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public override async Task<StaffDto?> Edit(StaffDto staffDto, string id)
        {
            try
            {
                var user = await db.User.FirstOrDefaultAsync(s => s.Id == id);
                if (user == null) throw new Exception("User not found");

                if (user.UserType != UserTypeEnum.Admin)
                    throw new Exception("Only Admin can update Staff");

                var guest = await db.Staffs
                    .FirstOrDefaultAsync(r => r.StaffId == staffDto.StaffId);
                if (guest == null) throw new Exception("Staff not found");
                var updated = await base.Edit(staffDto, id);

                return updated;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public override async Task<bool> Remove(int StaffId, string id)
        {
            try
            {
                var user = await db.User.FirstOrDefaultAsync(s => s.Id == id);
                if (user == null) throw new Exception("User not found");
                if (user.UserType != UserTypeEnum.Admin) throw new Exception("Only Admin can delete Staffs");
                var staff = db.Staffs.FirstOrDefault(g => g.StaffId == StaffId);
                if (staff == null) throw new Exception("staff not found");
                var result = await base.Remove(StaffId, id);
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }
        public async Task<object> GetAllStaffs()
        {
            var staffs = await db.Staffs.Select(g => new
            {
                g.StaffId,
                g.FirstName,
                g.LastName,
                g.Email,
                g.PhoneNumber,
                g.Address,
                g.Postion,
                user = new
                {
                    g.User!.Id,
                    g.User.UserName,
                    g.User.Email,
                    g.User.UserType
                }
            })
                .ToListAsync();
            return staffs;

        }
        public async Task<object> GetStaffById(int staffId)
        {
            var guest = await db.Staffs
                .Where(g => g.StaffId == staffId)
                .Select(g => new
                {
                    g.StaffId,
                    g.FirstName,
                    g.LastName,
                    g.Email,
                    g.PhoneNumber,
                    g.Address,
                    g.Postion,
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
    }
}
