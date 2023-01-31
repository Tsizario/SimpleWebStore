using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleWebStore.DAL.UnitOfWork;

namespace SimpleWebStore.DAL.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddDalServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseSqlServer(
                    configuration.GetConnectionString("SimpleWebStoreConnectionString"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return services;
        }
    }
}