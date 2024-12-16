using AutoMapper;
using NoPersaService.DTOs.Management.Mapped;
using NoPersaService.DTOs.Management.RA;
using NoPersaService.Models;
using NoPersaService.Util;

namespace NoPersaService.MappingProfiles
{
    public class ManagementProfile : Profile
    {
        public ManagementProfile() 
        {
            CreateMap<DTOCustomerOverview, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Name) ? null : src.Name))
                .ForMember(dest => dest.DeliveryLocation, opt => opt.MapFrom(src => src.DeliveryLocation))
                .ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.ArticleId, false)))
                .ForMember(dest => dest.DefaultNumberOfBoxes, opt => opt.MapFrom(src => src.DefaultNumberOfBoxes))
                .ForMember(dest => dest.MonthlyOverviews, opt => opt.MapFrom(src => src.MonthlyDeliveries))
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.RouteId, false)))
                .ForMember(dest => dest.CustomersLightDiets, opt => opt.MapFrom(src => src.LightDietOverviews))
                .ForMember(dest => dest.CustomersFoodWish, opt => opt.MapFrom(src => (src.FoodWishesOverviews ?? new List<DTOFoodWishesOverview>()).Concat(src.IngredientWishesOverviews ?? new List<DTOFoodWishesOverview>())))
                .ForMember(dest => dest.CustomerMenuPlans, opt => opt.MapFrom(src => src.BoxContentSelectedList));

            CreateMap<Customer, DTOCustomerOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DeliveryLocation, opt => opt.MapFrom(src => src.DeliveryLocation))
                .ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.ArticleId)))
                .ForMember(dest => dest.DefaultNumberOfBoxes, opt => opt.MapFrom(src => src.DefaultNumberOfBoxes))
                .ForMember(dest => dest.MonthlyDeliveries, opt => opt.MapFrom(src => src.MonthlyOverviews))
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.RouteId)))
                .ForMember(dest => dest.LightDietOverviews, opt => opt.MapFrom(src => src.CustomersLightDiets))
                .ForPath(dest => dest.FoodWishesOverviews, opt => opt.MapFrom(src => src.CustomersFoodWish.Where(cfw => !cfw.FoodWish.IsIngredient)))
                .ForPath(dest => dest.IngredientWishesOverviews, opt => opt.MapFrom(src => src.CustomersFoodWish.Where(cfw => cfw.FoodWish.IsIngredient)))
                .ForMember(dest => dest.BoxContentSelectedList, opt => opt.MapFrom(src => src.CustomerMenuPlans));

            CreateMap<DTOWeekdays, Weekday>();

            CreateMap<Weekday, DTOWeekdays>();

            CreateMap<DTOMonthlyDelivery, MonthlyOverview>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.MonthOfTheYear == null ? (int?)null : src.MonthOfTheYear.Year))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.MonthOfTheYear == null ? (int?)null : src.MonthOfTheYear.Month))
                .ForMember(dest => dest.DailyOverviews, opt => opt.MapFrom(src => src.DailyDeliveries));

            CreateMap<MonthlyOverview, DTOMonthlyDelivery>()
                .ForMember(dest => dest.MonthOfTheYear, opt => opt.MapFrom(src =>
                    new DTOMonthOfTheYear
                    {
                        Month = src.Month,
                        Year = src.Year
                    }))
                .ForMember(dest => dest.DailyDeliveries, opt => opt.MapFrom(src => src.DailyOverviews));

            CreateMap<DTODailyDelivery, DailyOverview>()
                .ForMember(dest => dest.DayOfMonth, opt => opt.MapFrom(src => src.DayOfMonth))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Price) ? (double?)null : double.Parse(src.Price)))
                .ForMember(dest => dest.NumberOfBoxes, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.NumberOfBoxes) ? (double?)null : double.Parse(src.NumberOfBoxes)));

            CreateMap<DailyOverview, DTODailyDelivery>()
                .ForMember(dest => dest.DayOfMonth, opt => opt.MapFrom(src => src.DayOfMonth))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.HasValue ? src.Price.Value.ToString() : null))
                .ForMember(dest => dest.NumberOfBoxes, opt => opt.MapFrom(src => src.NumberOfBoxes.HasValue ? src.NumberOfBoxes.Value.ToString() : null));

            CreateMap<DTODeliveryLocation, DeliveryLocation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Address) ? null : src.Address))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Region) ? null : src.Region))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => double.Parse(src.GeoLocation!.Split(new[] { ',' })[0])))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => double.Parse(src.GeoLocation!.Split(new[] { ',' })[1])))
                .ForMember(dest => dest.DeliveryWhishes, opt => opt.MapFrom(src => src.DeliveryWhishes));

            CreateMap<DeliveryLocation, DTODeliveryLocation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Address) ? null : src.Address))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Region) ? null : src.Region))
                .ForMember(dest => dest.GeoLocation, opt => opt.MapFrom(src => $"{src.Latitude}, {src.Longitude}"))
                .ForMember(dest => dest.DeliveryWhishes, opt => opt.MapFrom(src => src.DeliveryWhishes));

            CreateMap<DTOLightDietOverview, CustomersLightDiet>()
                .ForMember(dest => dest.LightDietId, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)));

            CreateMap<CustomersLightDiet, DTOLightDietOverview>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.LightDietId)))
                 .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.LightDiet!.Name));

            CreateMap<DTOFoodWishesOverview, CustomersFoodWish>()
                .ForMember(dest => dest.FoodWishId, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)));

            CreateMap<CustomersFoodWish, DTOFoodWishesOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.FoodWishId)))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.FoodWish!.Name));

            CreateMap<DTOBoxContentSelected, CustomersMenuPlan>()
                .ForMember(dest => dest.BoxContentId, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.PortionSizeId, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.PortionSizeId, false)));

            CreateMap<CustomersMenuPlan, DTOBoxContentSelected>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.BoxContentId)))
                .ForPath(dest => dest.Position, opt => opt.MapFrom(src => src.BoxContent.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.BoxContent.Name))
                .ForMember(dest => dest.PortionSizeId, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.PortionSizeId)));

            CreateMap<LightDiet, DTOLightDietOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Selected, opt => opt.MapFrom(src => false));

            CreateMap<FoodWish, DTOFoodWishesOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Selected, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsIngredient, opt => opt.MapFrom(src => src.IsIngredient));

            CreateMap<DTOMonthOfTheYear, MappedMonthOfTheYear>()
                .ForMember(dest => dest.ReferenceId, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.ReferenceId, false)))
                .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year));
        }
    }
}
