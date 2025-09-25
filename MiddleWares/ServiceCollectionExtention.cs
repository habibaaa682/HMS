using HMS.Services;

namespace HMS.MiddleWares
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddDIService(this IServiceCollection services)
        {
            services.AddScoped<IRoomServices, RoomServices>();
            services.AddScoped<IReservationServices, ReservationServices>();
            return services;
        }
    }
}
