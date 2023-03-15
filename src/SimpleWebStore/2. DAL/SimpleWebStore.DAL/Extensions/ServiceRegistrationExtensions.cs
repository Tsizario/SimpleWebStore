using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleWebStore.DAL.Helpers;
using SimpleWebStore.DAL.MapperProfiles;
using SimpleWebStore.DAL.UnitOfWork;
using SimpleWebStore.Domain.ExternalSettings;

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

            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<ProductProfile>();
                m.AddProfile<OrderHeaderProfile>();
                m.AddProfile<OrderDetailProfile>();
            });

            services.AddSingleton(s => mapperConfig.CreateMapper());

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddSingleton<IEmailSender, EmailSender>();

            return services;
        }
    }
}