using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
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
            services.AddSingleton<PresenceTracker>();

            services.AddScoped<LogUserActivity>();

            // Pull cloudinary settings from appsettings.json
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();

            services.Configure<FileRepoSettings>(config.GetSection("FileRepoSettings"));
            services.AddScoped<IFileRepoService, FileService>();
            
            // Add service for our repository
            // services.AddScoped<ILikesRespository, LikesRepository>();
            // services.AddScoped<IMessageRepository, MessageRepository>();
            // services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // services.AddSingleton // Uses resources until application stops
            // services.AddTransient //not for http request typical
            // services.AddScoped// Scope to the lifetime of the http request in this case JWT
            services.AddScoped<ITokenService, TokenService>();
            
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