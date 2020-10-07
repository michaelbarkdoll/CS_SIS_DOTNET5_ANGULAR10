using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // services.AddSingleton // Uses resources until application stops
            // services.AddTransient //not for http request typical
            // services.AddScoped// Scope to the lifetime of the http request in this case JWT
            services.AddScoped<ITokenService, TokenService>();
            // Add service for our repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);    // Allows automapper to create maps
            services.AddDbContext<DataContext>(options =>
            {
                //options.UseSqlite("Connection string");
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}