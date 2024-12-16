using AutoMapper;
using NoPersaService.DTOs.General.Answer;
using NoPersaService.DTOs.General.Mapped;
using NoPersaService.DTOs.General.Received;
using NoPersaService.Models;
using NoPersaService.Util;
using Route = NoPersaService.Models.Route;

namespace NoPersaService.MappingProfiles
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile() 
        {
            CreateMap<Customer, MappedIDString>();

            CreateMap<MappedIDString, DTOIDString>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<DTOId, MappedId>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, true)));

            CreateMap<Route, DTOSelectInput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));

            CreateMap<DTOSelectInput, PortionSize>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Value) ? null : src.Value));

            CreateMap<PortionSize, DTOSelectInput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));
        }
    }
}
