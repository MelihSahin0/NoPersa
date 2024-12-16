using AutoMapper;
using NoPersaService.DTOs.Delivery.Answer;
using NoPersaService.DTOs.Delivery.Mapped;
using NoPersaService.DTOs.Delivery.RA;
using NoPersaService.DTOs.Delivery.Receive;
using NoPersaService.Models;
using NoPersaService.Util;
using Route = NoPersaService.Models.Route;

namespace NoPersaService.MappingProfiles
{
    public class DeliveryProfile : Profile
    {
        public DeliveryProfile() 
        {
            CreateMap<DTORouteSummary, Route>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Name) ? null : src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position ?? (int?)null))
                .ForMember(dest => dest.IsDrivable, opt => opt.MapFrom(src => src.IsDrivable));

            CreateMap<Route, DTORouteSummary>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.IsDrivable, opt => opt.MapFrom(src => src.IsDrivable))
                .ForMember(dest => dest.NumberOfCustomers, opt => opt.MapFrom(src => src.Customers.Count()));

            CreateMap<Route, DTORouteOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.CustomerDeliveryStatus, opt => opt.MapFrom(src => src.Customers));

            CreateMap<Customer, DTOCustomerDeliveryStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

            CreateMap<Route, DTOCustomersInRoute>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CustomerSequence, opt => opt.MapFrom(src => src.Customers));

            CreateMap<Customer, DTOCustomerSequence>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

            CreateMap<DTOSelectedDayWithReference, MappedSelectedDayWithReference>()
                .ForMember(dest => dest.ReferenceId, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.ReferenceId, false)))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
                .ForMember(dest => dest.GeoLocation, opt => opt.MapFrom(src => src.GeoLocation));

            CreateMap<Customer, DTOCustomersLocation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.Address, opt => opt.MapFrom(src => src.DeliveryLocation!.Address))
                .ForPath(dest => dest.Latitude, opt => opt.MapFrom(src => src.DeliveryLocation!.Latitude))
                .ForPath(dest => dest.Longitude, opt => opt.MapFrom(src => src.DeliveryLocation!.Longitude))
                .ForPath(dest => dest.DeliveryWishes, opt => opt.MapFrom(src => src.DeliveryLocation!.DeliveryWhishes))
                .ForMember(dest => dest.NumberOfBoxes, opt => opt.MapFrom(src => src.DefaultNumberOfBoxes));
        }
    }
}
