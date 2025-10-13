using AutoMapper;
using HMS.Models;
using HMS.Models.DTO;
using HMS.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public interface IServiceServices : IBaseBusinessService< Service,ServiceDto>
    {
        Task<object> GetAllServices();
        Task<object> GetServiceById(int ServiceId);
        Task<bool> ApplyService(int serviceId, string userId, int guestId, int staffId);
    }
    public class ServiceServices(HMSContext db, IMapper mapper) : BaseBusinessService<Service, ServiceDto>(mapper, db), IServiceServices
    {
        public override async Task<ServiceDto?> Insert(ServiceDto serviceDto, string id)
        {
            try
            {
                var user = await _db.User.FirstOrDefaultAsync(s => s.Id == id);
                if (user == null) throw new Exception("User not found");
                if (user.UserType != UserTypeEnum.Admin) throw new Exception("Only Admin can add Service");
                serviceDto.UserId = id;
                var result = await base.Insert(serviceDto, id);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public override async Task<ServiceDto?> Edit(ServiceDto serviceDto, string id)
        {
            try
            {
                var user = await db.User.FirstOrDefaultAsync(s => s.Id == id);
                if (user == null) throw new Exception("User not found");
                if (user.UserType != UserTypeEnum.Admin)
                    throw new Exception("Only Admin can update Service");
                var service = await db.Services
                    .FirstOrDefaultAsync(r => r.ServiceId == serviceDto.ServiceId);
                if (service == null) throw new Exception("Service not found");
                serviceDto.UserId = id;
                var updated = await base.Edit(serviceDto, id);
                return updated;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public override async Task<bool> Remove(int ServiceId, string id)
        {
            try
            {
                var user = await db.User.FirstOrDefaultAsync(s => s.Id == id);
                if (user == null) throw new Exception("User not found");
                if (user.UserType != UserTypeEnum.Admin) throw new Exception("Only Admin can delete Services");
                var staff = db.Staffs.FirstOrDefault(g => g.StaffId == ServiceId);
                if (staff == null) throw new Exception("Service not found");
                var result = await base.Remove(ServiceId, id);
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }
        public async Task<object> GetAllServices()
        {
            var staffs = await db.Services
                .Select(g => new
                {
                    g.ServiceId,
                    g.ServiceName,
                    g.Price,
                    CreatedBy = g.UserServices!
                        .Where(us => us.User!.UserType == UserTypeEnum.Admin)
                        .Select(us => us.User!.UserName)
                        .FirstOrDefault(),

                                ServedBy = g.UserServices!
                        .Where(us => us.User!.UserType == UserTypeEnum.Staff)
                        .Select(us => us.User!.UserName)
                        .FirstOrDefault(),

                                UsedBy = g.UserServices!
                        .Where(us => us.User!.UserType == UserTypeEnum.Guest)
                        .Select(us => us.User!.UserName)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return staffs;

        }
        public async Task<object> GetServiceById(int ServiceId)
        {
            var guest = await db.Services
                .Where(g => g.ServiceId == ServiceId)
                .Select(g => new
                {
                    g.ServiceId,
                    g.ServiceName,
                    g.Price,
                    CreatedBy = g.UserServices!
                        .Where(us => us.User!.UserType == UserTypeEnum.Admin)
                        .Select(us => us.User!.UserName)
                        .FirstOrDefault(),

                                ServedBy = g.UserServices!
                        .Where(us => us.User!.UserType == UserTypeEnum.Staff)
                        .Select(us => us.User!.UserName)
                        .FirstOrDefault(),

                                UsedBy = g.UserServices!
                        .Where(us => us.User!.UserType == UserTypeEnum.Guest)
                        .Select(us => us.User!.UserName)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();
            return guest!;

        }

        public async Task<bool> ApplyService(int serviceId, string userId, int guestId, int staffId)
        {
            try
            {
                var user = await db.User.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null) throw new Exception("User not found");
                var service = await db.Services.FirstOrDefaultAsync(s => s.ServiceId == serviceId);
                if (service == null) throw new Exception("Service not found");
                var userService = new UserService
                {
                    ServiceId = serviceId,
                    UserId = userId,
                };
                await db.UserServices.AddAsync(userService);
                var guestService = new
                {
                    service.ServiceId,
                    service.ServiceName,
                    service.Price,
                    guestId,
                };
                await db.AddAsync(guestService);
                var staffService = new
                {
                    service.ServiceId,
                    service.ServiceName,
                    service.Price,
                    staffId,
                };
                await db.AddAsync(staffService);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
