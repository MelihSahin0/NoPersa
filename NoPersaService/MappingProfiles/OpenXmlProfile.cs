using AutoMapper;
using NoPersaService.Models;
using OpenXml.Models;

namespace NoPersaService.MappingProfiles
{
    public class OpenXmlProfile : Profile
    {
        public OpenXmlProfile()
        {
            CreateMap<MonthlyOverview, Invoice>()
                .ForPath(dest => dest.RouteName, opt => opt.MapFrom(src => src.Customer!.Route!.Name))
                .ForPath(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.Customer!.SerialNumber ?? string.Empty))
                .ForPath(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer!.Name))
                .ForMember(dest => dest.TotalSales, opt => opt.MapFrom(src => src.TotalSales))
                .ForPath(dest => dest.DailySales, opt => opt.MapFrom(src => src.DailyOverviews == null ? new List<double>() : src.DailyOverviews.OrderBy(d => d.DayOfMonth).Select(d => d.TotalSales)));

            CreateMap<Customer, ExcelCustomer>()
                .ForPath(dest => dest.RouteName, opt => opt.MapFrom(src => src.Route!.Name))
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.SerialNumber ?? string.Empty))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title ?? string.Empty))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.Address, opt => opt.MapFrom(src => src.DeliveryLocation!.Address))
                .ForPath(dest => dest.Region, opt => opt.MapFrom(src => src.DeliveryLocation!.Region))
                .ForPath(dest => dest.Latitude, opt => opt.MapFrom(src => src.DeliveryLocation!.Latitude))
                .ForPath(dest => dest.Longitude, opt => opt.MapFrom(src => src.DeliveryLocation!.Longitude))
                .ForPath(dest => dest.DeliveryWhishes, opt => opt.MapFrom(src => src.DeliveryLocation!.DeliveryWhishes ?? string.Empty))
                .ForMember(dest => dest.ContactInformation, opt => opt.MapFrom(src => src.ContactInformation ?? string.Empty))
                .ForPath(dest => dest.Article, opt => opt.MapFrom(src => src.Article!.Name))
                .ForMember(dest => dest.DefaultNumberOfBoxes, opt => opt.MapFrom(src => src.DefaultNumberOfBoxes))
                .ForPath(dest => dest.WMonday, opt => opt.MapFrom(src => src.Workdays!.Monday))
                .ForPath(dest => dest.WTuesday, opt => opt.MapFrom(src => src.Workdays!.Tuesday))
                .ForPath(dest => dest.WWednesday, opt => opt.MapFrom(src => src.Workdays!.Wednesday))
                .ForPath(dest => dest.WThursday, opt => opt.MapFrom(src => src.Workdays!.Thursday))
                .ForPath(dest => dest.WFriday, opt => opt.MapFrom(src => src.Workdays!.Friday))
                .ForPath(dest => dest.WSaturday, opt => opt.MapFrom(src => src.Workdays!.Saturday))
                .ForPath(dest => dest.WSunday, opt => opt.MapFrom(src => src.Workdays!.Sunday))
                .ForPath(dest => dest.HMonday, opt => opt.MapFrom(src => src.Holidays!.Monday))
                .ForPath(dest => dest.HTuesday, opt => opt.MapFrom(src => src.Holidays!.Tuesday))
                .ForPath(dest => dest.HWednesday, opt => opt.MapFrom(src => src.Holidays!.Wednesday))
                .ForPath(dest => dest.HThursday, opt => opt.MapFrom(src => src.Holidays!.Thursday))
                .ForPath(dest => dest.HFriday, opt => opt.MapFrom(src => src.Holidays!.Friday))
                .ForPath(dest => dest.HSaturday, opt => opt.MapFrom(src => src.Holidays!.Saturday))
                .ForPath(dest => dest.HSunday, opt => opt.MapFrom(src => src.Holidays!.Sunday))
                .ForPath(dest => dest.Menus, opt => opt.MapFrom(src => src.CustomerMenuPlans.OrderBy(cmp => cmp.BoxContent!.Position).Select(cmp => cmp.PortionSize!.Name).ToList()))
                .ForPath(dest => dest.LightDiets, static opt => opt.MapFrom(src => src.CustomersLightDiets.Count != 0 ? Enumerable.Range(0, src.CustomersLightDiets.Max(cld => cld.LightDiet!.Position) + 1)
                                                                            .Select(pos => src.CustomersLightDiets.Any(cld => cld.LightDiet!.Position == pos))
                                                                            .ToList() : new List<bool>()));
        }
    }
}
