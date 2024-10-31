using AutoMapper;
using SharedLibrary.DTOs;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class RouteProfile : Profile
    {
        public RouteProfile() 
        {
            CreateMap<DTORouteOverview, Route>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == -1 ? 0 : src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Name) ? null : src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position ?? (int?)null));

            CreateMap<Route, DTORouteOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.NumberOfCustomers, opt => opt.MapFrom(src => src.Customers.Count()));

            CreateMap<Route, DTORouteDetails>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.CustomersRoute, opt => opt.MapFrom(src => src.Customers));

            CreateMap<Customer, DTOCustomerRoute>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

            CreateMap<Route, DTOSequenceDetails>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CustomersRoute, opt => opt.MapFrom(src => src.Customers));

            CreateMap<Customer, DTOCustomerSequence>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));
        }
    }
}
