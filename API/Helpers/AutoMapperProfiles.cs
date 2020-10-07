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
        }
    }
}