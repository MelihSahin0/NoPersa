using AutoMapper;
using NoPersaService.DTOs.Box.Mapped;
using NoPersaService.DTOs.Box.RA;
using NoPersaService.DTOs.Box.Receive;
using NoPersaService.Models;
using NoPersaService.Util;

namespace NoPersaService.MappingProfiles
{
    public class BoxProfile : Profile
    {
        public BoxProfile() 
        {
            CreateMap<DTOBoxStatus, BoxStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.NumberOfBoxesPreviousDay, opt => opt.MapFrom(src => src.NumberOfBoxesPreviousDay))
                .ForMember(dest => dest.DeliveredBoxes, opt => opt.MapFrom(src => src.DeliveredBoxes))
                .ForMember(dest => dest.ReceivedBoxes, opt => opt.MapFrom(src => src.ReceivedBoxes))
                .ForMember(dest => dest.NumberOfBoxesCurrentDay, opt => opt.MapFrom(src => src.NumberOfBoxesCurrentDay));

            CreateMap<BoxStatus, DTOBoxStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.NumberOfBoxesPreviousDay, opt => opt.MapFrom(src => src.NumberOfBoxesPreviousDay))
                .ForMember(dest => dest.DeliveredBoxes, opt => opt.MapFrom(src => src.DeliveredBoxes))
                .ForMember(dest => dest.ReceivedBoxes, opt => opt.MapFrom(src => src.ReceivedBoxes))
                .ForMember(dest => dest.NumberOfBoxesCurrentDay, opt => opt.MapFrom(src => src.NumberOfBoxesCurrentDay))
                .ForPath(dest => dest.CustomersName, opt => opt.MapFrom(src => src.Customer!.Name))
                .ForPath(dest => dest.RouteName, opt => opt.MapFrom(src => src.Customer!.Route!.Name));

            CreateMap<DTOCustomersBoxStatus, MappedCustomerBoxStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.DeliveredBoxes, opt => opt.MapFrom(src => src.DeliveredBoxes))
                .ForMember(dest => dest.ReceivedBoxes, opt => opt.MapFrom(src => src.ReceivedBoxes));
        }
    }
}
