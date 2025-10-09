using HMS.Services;

namespace HMS.MiddleWares
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddDIService(this IServiceCollection services)
        {
            services.AddScoped<IRoomServices, RoomServices>();
            services.AddScoped<IReservationServices, ReservationServices>();
            services.AddScoped<IGuestServices, GuestServices>();
            services.AddScoped<IStaffServices, StaffServices>();
            return services;
        }
    }
}
