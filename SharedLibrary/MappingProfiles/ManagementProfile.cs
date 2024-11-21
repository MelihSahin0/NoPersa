using AutoMapper;
using SharedLibrary.DTOs.GetDTOs;
using SharedLibrary.DTOs.Management;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class ManagementProfile : Profile
    {
        public ManagementProfile() 
        {
            CreateMap<DTOCustomerOverview, Customer>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == -1 ? 0 : src.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Name) ? null : src.Name))
               .ForMember(dest => dest.DeliveryLocation, opt => opt.MapFrom(src => src.DeliveryLocation))
               .ForMember(dest => dest.DefaultNumberOfBoxes, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DefaultNumberOfBoxes) ? (int?)null : int.Parse(src.DefaultNumberOfBoxes)))
               .ForMember(dest => dest.MonthlyOverviews, opt => opt.MapFrom(src => src.MonthlyDeliveries))
               .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId ?? (int?)null))
               .ForMember(dest => dest.CustomersLightDiets, opt => opt.MapFrom(src => src.LightDietOverviews))
               .ForMember(dest => dest.CustomerMenuPlans, opt => opt.MapFrom(src => src.BoxContentSelectedList));

            CreateMap<Customer, DTOCustomerOverview>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DeliveryLocation, opt => opt.MapFrom(src => src.DeliveryLocation))
                .ForMember(dest => dest.DefaultNumberOfBoxes, opt => opt.MapFrom(src => src.DefaultNumberOfBoxes))
                .ForMember(dest => dest.MonthlyDeliveries, opt => opt.MapFrom(src => src.MonthlyOverviews))
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId ?? (int?)null))
                .ForMember(dest => dest.LightDietOverviews, opt => opt.MapFrom(src => src.CustomersLightDiets))
                .ForMember(dest => dest.BoxContentSelectedList, opt => opt.MapFrom(src => src.CustomerMenuPlans));

            CreateMap<DTOWeekdays, Weekday>();

            CreateMap<Weekday, DTOWeekdays>();

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

            CreateMap<DTODailyDelivery, DailyOverview>()
                .ForMember(dest => dest.DayOfMonth, opt => opt.MapFrom(src => src.DayOfMonth))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Price) ? (double?)null : double.Parse(src.Price)))
                .ForMember(dest => dest.NumberOfBoxes, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.NumberOfBoxes) ? (double?)null : double.Parse(src.NumberOfBoxes)));

            CreateMap<DailyOverview, DTODailyDelivery>()
                .ForMember(dest => dest.DayOfMonth, opt => opt.MapFrom(src => src.DayOfMonth))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.HasValue ? src.Price.Value.ToString() : null))
                .ForMember(dest => dest.NumberOfBoxes, opt => opt.MapFrom(src => src.NumberOfBoxes.HasValue ? src.NumberOfBoxes.Value.ToString() : null));

            CreateMap<DTODeliveryLocation, DeliveryLocation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == -1 ? 0 : src.Id))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Address) ? null : src.Address))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Region) ? null : src.Region))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => double.Parse(src.GeoLocation!.Split(new[] { ',' })[0])))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => double.Parse(src.GeoLocation!.Split(new[] { ',' })[1])))
                .ForMember(dest => dest.DeliveryWhishes, opt => opt.MapFrom(src => src.DeliveryWhishes));

            CreateMap<DeliveryLocation, DTODeliveryLocation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == -1 ? 0 : src.Id))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Address) ? null : src.Address))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Region) ? null : src.Region))
                .ForMember(dest => dest.GeoLocation, opt => opt.MapFrom(src => $"{src.Latitude}, {src.Longitude}"))
                .ForMember(dest => dest.DeliveryWhishes, opt => opt.MapFrom(src => src.DeliveryWhishes));

            CreateMap<DTOArticle, Article>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.NewPrice));

            CreateMap<Article, DTOArticle>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.NewPrice));
        }
    }
}
