using AutoMapper;
using SharedLibrary.DTOs;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class RouteProfile : Profile
    {
        public RouteProfile() 
        {
            CreateMap<DTORoute, Route>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == -1 ? 0 : src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Name) ? null : src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.HasValue ? src.Position.Value : (int?)null)) 
                .ForMember(dest => dest.Customers, opt => opt.MapFrom(src => src.CustomersRoute));

            CreateMap<Route, DTORoute>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.CustomersRoute, opt => opt.MapFrom(src => src.Customers != null ? src.Customers.Select(c => new DTOCustomerRoute
                { 
                    Id = c.Id,
                    Name = c.Name,
                    Position = c.Position
                }).ToArray() : null));

            CreateMap<Customer, DTOCustomerRoute>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

            CreateMap<DTOCustomerRoute, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

        }
    }
}
