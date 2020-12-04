using System;
using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        // Allows us to map from one object to another (that is all it does)
        public AutoMapperProfiles()
        {
            //CreateMap<MapFrom, To>();
            CreateMap<AppUser, MemberDto>()
                // Used to map dest .PhotoUrl to LINQ query FROM src.Photos.FirstOrDefault check if it isMain
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                // Use API.Extension method CalculateAge to calculate src.DateOfBirth's dest.Age
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            //CreateMap<MapFrom, To>();
            CreateMap<Photo, PhotoDto>();
            CreateMap<UserFile, UserFileDto>();
            //CreateMap<MapFrom, To>();
            CreateMap<MemberUpdateDto, AppUser>();
            //CreateMap<MapFrom, To>(); // Now we don't need to manually map the properties in our account controller
            CreateMap<RegisterDto, AppUser>();
            
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));

            // CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));    // Add z to end of dates for client

            CreateMap<PrintJob, PrintJobDto>();
            CreateMap<AppUser, MemberPrintJobDto>();

            CreateMap<AppUser, MemberPrintQuotaDto>();

            CreateMap<Printer, PrinterDto>();
            CreateMap<PrintJob, MemberPrintJobDto>();

            CreateMap<MemberUpdateUrlDto, AppUser>();
            CreateMap<MemberRolesDto, AppUser>();

            CreateMap<AppUser, MemberAdminViewDto>();
            
            CreateMap<AppUserRole, MemberAdminRoleView>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.Role.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

                //Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
            CreateMap<AppUser, MemberAdminRoleView>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Id))
                // .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));
                // .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.UserRoles.ToList()));
                // .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.UserRoles.Where(c => c.User.Id == src.Id)));
        }
    }
}