using AutoMapper;
using SharedLibrary.DTOs;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile() 
        {
            CreateMap<DTOCustomer, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == -1 ? 0 : src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Name) ? null : src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Address) ? null : src.Address))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Region) ? null : src.Region))
                .ForMember(dest => dest.Article, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Article) ? (int?)null : int.Parse(src.Article)))
                .ForMember(dest => dest.DefaultPrice, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DefaultPrice) ? (double?)null : double.Parse(src.DefaultPrice.Replace(",", "."))))
                .ForMember(dest => dest.DefaultNumberOfBoxes, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DefaultNumberOfBoxes) ? (int?)null : int.Parse(src.DefaultNumberOfBoxes)))
                .ForMember(dest => dest.MonthlyOverviews, opt => opt.MapFrom(src => src.MonthlyDeliveries))
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId ?? (int?)null));

            CreateMap<Customer, DTOCustomer>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
                .ForMember(dest => dest.Article, opt => opt.MapFrom(src => src.Article))
                .ForMember(dest => dest.DefaultPrice, opt => opt.MapFrom(src => src.DefaultPrice))
                .ForMember(dest => dest.DefaultNumberOfBoxes, opt => opt.MapFrom(src => src.DefaultNumberOfBoxes))
                .ForMember(dest => dest.MonthlyDeliveries, opt => opt.MapFrom(src => src.MonthlyOverviews))
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId ?? (int?)null));
        }
    }
}
