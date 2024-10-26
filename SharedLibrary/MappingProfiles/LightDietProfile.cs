using AutoMapper;
using SharedLibrary.DTOs;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class LightDietProfile : Profile
    {
        public LightDietProfile()
        {
            CreateMap<DTOLightDiet, LightDiet>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value));
             
            CreateMap<LightDiet, DTOLightDiet>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));

            CreateMap<DTOLightDietOverview, CustomersLightDiet>()
                .ForMember(dest => dest.LightDietId, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.LightDiet.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Selected, opt => opt.MapFrom(src => src.Selected));

            CreateMap<CustomersLightDiet, DTOLightDietOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LightDietId))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.LightDiet.Name))
                .ForMember(dest => dest.Selected, opt => opt.MapFrom(src => src.Selected));
        }
    }
}
