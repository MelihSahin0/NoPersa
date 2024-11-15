using AutoMapper;
using SharedLibrary.DTOs.Management;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile() 
        {
            CreateMap<DTOCustomerOverview, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == -1 ? 0 : src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Name) ? null : src.Name))
                .ForMember(dest => dest.DeliveryLocation, opt => opt.MapFrom(src => src.DeliveryLocation))
                .ForMember(dest => dest.Article, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Article) ? (int?)null : int.Parse(src.Article)))
                .ForMember(dest => dest.DefaultPrice, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DefaultPrice) ? (double?)null : double.Parse(src.DefaultPrice.Replace(",", "."))))
                .ForMember(dest => dest.DefaultNumberOfBoxes, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DefaultNumberOfBoxes) ? (int?)null : int.Parse(src.DefaultNumberOfBoxes)))
                .ForMember(dest => dest.MonthlyOverviews, opt => opt.MapFrom(src => src.MonthlyDeliveries))
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId ?? (int?)null))
                .ForMember(dest => dest.CustomersLightDiets, opt => opt.MapFrom(src => src.LightDietOverviews))
                .ForMember(dest => dest.CustomerMenuPlans, opt => opt.MapFrom(src => src.BoxContentSelectedList));

            CreateMap<Customer, DTOCustomerOverview>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DeliveryLocation, opt => opt.MapFrom(src => src.DeliveryLocation))
                .ForMember(dest => dest.Article, opt => opt.MapFrom(src => src.Article))
                .ForMember(dest => dest.DefaultPrice, opt => opt.MapFrom(src => src.DefaultPrice))
                .ForMember(dest => dest.DefaultNumberOfBoxes, opt => opt.MapFrom(src => src.DefaultNumberOfBoxes))
                .ForMember(dest => dest.MonthlyDeliveries, opt => opt.MapFrom(src => src.MonthlyOverviews))
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId ?? (int?)null))
                .ForMember(dest => dest.LightDietOverviews, opt => opt.MapFrom(src => src.CustomersLightDiets))
                .ForMember(dest => dest.BoxContentSelectedList, opt => opt.MapFrom(src => src.CustomerMenuPlans));
        }
    }
}
