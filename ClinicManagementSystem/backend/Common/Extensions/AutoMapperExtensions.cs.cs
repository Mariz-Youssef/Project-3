namespace ClinicManagementSystem.backend.Common.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
