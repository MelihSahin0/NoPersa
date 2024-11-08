using AutoMapper;
using SharedLibrary.DTOs.Management;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class WeekdaysProfile : Profile
    {
        public WeekdaysProfile() 
        {
            CreateMap<DTOWeekdays, Weekday>();

            CreateMap<Weekday, DTOWeekdays>();
        }
    }
}
