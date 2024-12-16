using AutoMapper;
using NoPersaService.DTOs.Gastro.Answer;
using NoPersaService.DTOs.Gastro.RA;
using NoPersaService.Models;
using NoPersaService.Util;

namespace NoPersaService.MappingProfiles
{
    public class GastronomyProfile : Profile
    {
        public GastronomyProfile()
        {
            CreateMap<DTOLightDiet, LightDiet>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value));
             
            CreateMap<LightDiet, DTOLightDiet>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));
            
            CreateMap<DTOBoxContent, BoxContent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value));

            CreateMap<BoxContent, DTOBoxContent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));

            CreateMap<DTOPortionSize, PortionSize>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));

            CreateMap<PortionSize, DTOPortionSize>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));

            CreateMap<DTOFoodWish, FoodWish>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsIngredient, opt => opt.MapFrom(src => src.IsIngredient));

            CreateMap<FoodWish, DTOFoodWish>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsIngredient, opt => opt.MapFrom(src => src.IsIngredient));

            CreateMap<LightDiet, DTOLightDietSummary>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => 0));

            CreateMap<BoxContent, DTOBoxContentSummary>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<PortionSize, DTOPortionSizeSummary>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => 0));
        }
    }
}
