using AutoMapper;
using SharedLibrary.DTOs;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class DailyOverviewProfile : Profile
    {
        public DailyOverviewProfile() 
        {
            CreateMap<DTODailyDelivery, DailyOverview>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Price) ? (double?)null : double.Parse(src.Price)))
                .ForMember(dest => dest.NumberOfBoxes, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.NumberOfBoxes) ? (double?)null : double.Parse(src.NumberOfBoxes)));

            CreateMap<DailyOverview, DTODailyDelivery>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.HasValue ? src.Price.Value.ToString() : null))
                .ForMember(dest => dest.NumberOfBoxes, opt => opt.MapFrom(src => src.NumberOfBoxes.HasValue ? src.NumberOfBoxes.Value.ToString() : null));
        }
    }
}
