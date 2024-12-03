using AutoMapper;
using SharedLibrary.DTOs.Gastro;
using SharedLibrary.DTOs.Management;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class GastronomyProfile : Profile
    {
        public GastronomyProfile()
        {
            CreateMap<DTOLightDiet, LightDiet>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value));
             
            CreateMap<LightDiet, DTOLightDiet>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));
            
            CreateMap<DTOBoxContent, BoxContent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value));

            CreateMap<BoxContent, DTOBoxContent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));

            CreateMap<DTOPortionSize, PortionSize>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));

            CreateMap<PortionSize, DTOPortionSize>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));

            CreateMap<DTOFoodWish, FoodWish>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsIngredient, opt => opt.MapFrom(src => src.IsIngredient));

            CreateMap<FoodWish, DTOFoodWish>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsIngredient, opt => opt.MapFrom(src => src.IsIngredient));

            CreateMap<DTOLightDietOverview, CustomersLightDiet>()
               .ForMember(dest => dest.LightDietId, opt => opt.MapFrom(src => src.Id))
               .ForPath(dest => dest.LightDiet.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<CustomersLightDiet, DTOLightDietOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LightDietId))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.LightDiet.Name));

            CreateMap<DTOFoodWishesOverview, CustomersFoodWish>()
               .ForMember(dest => dest.FoodWishId, opt => opt.MapFrom(src => src.Id))
               .ForPath(dest => dest.FoodWish.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<CustomersFoodWish, DTOFoodWishesOverview>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FoodWishId))
               .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.FoodWish.Name));

            CreateMap<DTOBoxContentSelected, CustomersMenuPlan>()
                .ForMember(dest => dest.BoxContentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PortionSizeId, opt => opt.MapFrom(src => src.PortionSizeId));

            CreateMap<CustomersMenuPlan, DTOBoxContentSelected>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BoxContentId))
                .ForPath(dest => dest.Position, opt => opt.MapFrom(src => src.BoxContent.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.BoxContent.Name))
                .ForMember(dest => dest.PortionSizeId, opt => opt.MapFrom(src => src.PortionSizeId));

            CreateMap<LightDiet, DTOLightDietSummary>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => 0));

            CreateMap<BoxContent, DTOBoxContentSummary>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<PortionSize, DTOPortionSizeSummary>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => 0));
        }
    }
}
