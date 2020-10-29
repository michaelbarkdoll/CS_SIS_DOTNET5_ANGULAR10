using System;
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
                //options.UseSqlite(config.GetConnectionString("DefaultConnection"));
                // options.UseNpgsql(config.GetConnectionString("DefaultConnection"));

                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                string connStr;

                // Depending on if in development or production, use either Heroku-provided
                // connection string, or development connection string from env var.
                if (env == "Development")
                {
                    // Use connection string from file.
                    connStr = config.GetConnectionString("DefaultConnection");
                }
                else    // Heroku
                {
                    // Use connection string provided at runtime by Heroku.
                    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                    // Parse connection URL to connection string for Npgsql
                    connUrl = connUrl.Replace("postgres://", string.Empty);
                    var pgUserPass = connUrl.Split("@")[0];
                    var pgHostPortDb = connUrl.Split("@")[1];
                    var pgHostPort = pgHostPortDb.Split("/")[0];
                    var pgDb = pgHostPortDb.Split("/")[1];
                    var pgUser = pgUserPass.Split(":")[0];
                    var pgPass = pgUserPass.Split(":")[1];
                    var pgHost = pgHostPort.Split(":")[0];
                    var pgPort = pgHostPort.Split(":")[1];

                    connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}";
                }

                // Whether the connection string came from the local development configuration file
                // or from the environment variable from Heroku, use it to set up your DbContext.
                options.UseNpgsql(connStr);
            });

            return services;
        }
    }
}