using AutoMapper;
using SharedLibrary.DTOs.Delivery;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class DeliveryProfile : Profile
    {
        public DeliveryProfile() 
        {
            CreateMap<DTORouteSummary, Route>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == -1 ? 0 : src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Name) ? null : src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position ?? (int?)null))
                .ForMember(dest => dest.IsDrivable, opt => opt.MapFrom(src => src.IsDrivable));

            CreateMap<Route, DTORouteSummary>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.IsDrivable, opt => opt.MapFrom(src => src.IsDrivable))
                .ForMember(dest => dest.NumberOfCustomers, opt => opt.MapFrom(src => src.Customers.Count()));

            CreateMap<Route, DTORouteOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.CustomerDeliveryStatus, opt => opt.MapFrom(src => src.Customers));

            CreateMap<Customer, DTOCustomerDeliveryStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

            CreateMap<Route, DTOCustomersInRoute>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CustomerSequence, opt => opt.MapFrom(src => src.Customers));

            CreateMap<Customer, DTOCustomerSequence>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

            CreateMap<DTOBoxStatus, BoxStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NumberOfBoxesPreviousDay, opt => opt.MapFrom(src => src.NumberOfBoxesPreviousDay))
                .ForMember(dest => dest.DeliveredBoxes, opt => opt.MapFrom(src => src.DeliveredBoxes))
                .ForMember(dest => dest.ReceivedBoxes, opt => opt.MapFrom(src => src.ReceivedBoxes))
                .ForMember(dest => dest.NumberOfBoxesCurrentDay, opt => opt.MapFrom(src => src.NumberOfBoxesCurrentDay));

            CreateMap<BoxStatus, DTOBoxStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NumberOfBoxesPreviousDay, opt => opt.MapFrom(src => src.NumberOfBoxesPreviousDay))
                .ForMember(dest => dest.DeliveredBoxes, opt => opt.MapFrom(src => src.DeliveredBoxes))
                .ForMember(dest => dest.ReceivedBoxes, opt => opt.MapFrom(src => src.ReceivedBoxes))
                .ForMember(dest => dest.NumberOfBoxesCurrentDay, opt => opt.MapFrom(src => src.NumberOfBoxesCurrentDay))
                .ForPath(dest => dest.CustomersName, opt => opt.MapFrom(src => src.Customer.Name))
                .ForPath(dest => dest.RouteName, opt => opt.MapFrom(src => src.Customer.Route!.Name));
        }
    }
}
