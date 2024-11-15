using AutoMapper;
using SharedLibrary.DTOs.Management;
using SharedLibrary.Models;
using SharedLibrary.Validations;

namespace SharedLibrary.MappingProfiles
{
    public class DeliveryLocationProfile : Profile
    {
        public DeliveryLocationProfile()
        {
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
               .ForMember(dest => dest.GeoLocation, opt => opt.MapFrom(src => $"{src.Latitude}, {src.Longitude}" ))
               .ForMember(dest => dest.DeliveryWhishes, opt => opt.MapFrom(src => src.DeliveryWhishes));
        }
    }
}
