﻿using AutoMapper;
using SharedLibrary.DTOs;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class MonthlyOverviewProfile : Profile
    {
        public MonthlyOverviewProfile() 
        {
            CreateMap<DTOMonthlyDelivery, MonthlyOverview>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.MonthOfTheYear == null ? (int?)null : string.IsNullOrWhiteSpace(src.MonthOfTheYear.Year) ? (int?)null : int.Parse(src.MonthOfTheYear.Year)))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.MonthOfTheYear == null ? (int?)null : src.MonthOfTheYear.Month));

            CreateMap<MonthlyOverview, DTOMonthlyDelivery>()
                .ForMember(dest => dest.MonthOfTheYear, opt => opt.MapFrom(src =>
                    new DTOMonthOfTheYear
                    {
                        Month = src.Month,
                        Year = src.Year.ToString()
                    }));
        }
    }
}