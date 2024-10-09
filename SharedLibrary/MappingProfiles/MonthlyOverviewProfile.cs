using AutoMapper;
using SharedLibrary.DTOs;
using SharedLibrary.DTOs.GetDTOs;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class MonthlyOverviewProfile : Profile
    {
        public MonthlyOverviewProfile() 
        {
            CreateMap<DTOMonthlyDelivery, MonthlyOverview>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.MonthOfTheYear == null ? (int?)null : src.MonthOfTheYear.Year))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.MonthOfTheYear == null ? (int?)null : src.MonthOfTheYear.Month))
                .ForMember(dest => dest.DailyOverviews, opt => opt.MapFrom(src => src.DailyDeliveries));

            CreateMap<MonthlyOverview, DTOMonthlyDelivery>()
                .ForMember(dest => dest.MonthOfTheYear, opt => opt.MapFrom(src =>
                    new DTOMonthOfTheYear
                    {
                        Month = src.Month,
                        Year = src.Year
                    }))
                .ForMember(dest => dest.DailyDeliveries, opt => opt.MapFrom(src => src.DailyOverviews));
        }
    }
}
