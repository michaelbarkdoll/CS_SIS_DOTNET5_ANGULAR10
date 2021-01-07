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
            CreateMap<AppUser, MemberFileDto>();
            // CreateMap<Course, Course>();
            CreateMap<Course, CourseDto>();
            // CreateMap<Semester, Semester>();
            CreateMap<Semester, SemesterDto>();

            CreateMap<AppUser, MemberSshKeysDto>();
            CreateMap<MemberUpdateSshDto, AppUser>();
        }
    }
}