using AutoMapper;
using SharedLibrary.DTOs.Gastro;
using SharedLibrary.DTOs.Management;
using SharedLibrary.Models;

namespace SharedLibrary.MappingProfiles
{
    public class BoxConfigurationProfile : Profile
    {
        public BoxConfigurationProfile()
        {
            CreateMap<DTOLightDiet, LightDiet>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value));
             
            CreateMap<LightDiet, DTOLightDiet>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));
            
            CreateMap<DTOBoxContent, BoxContent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value));

            CreateMap<BoxContent, DTOBoxContent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));

            CreateMap<DTOPortionSize, PortionSize>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

            CreateMap<PortionSize, DTOPortionSize>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

            CreateMap<DTOLightDietOverview, CustomersLightDiet>()
                .ForMember(dest => dest.LightDietId, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.LightDiet.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Selected, opt => opt.MapFrom(src => src.Selected));

            CreateMap<CustomersLightDiet, DTOLightDietOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LightDietId))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.LightDiet.Name))
                .ForMember(dest => dest.Selected, opt => opt.MapFrom(src => src.Selected));

            CreateMap<DTOBoxContentSelected, CustomersMenuPlan>()
                .ForMember(dest => dest.BoxContentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PortionSizeId, opt => opt.MapFrom(src => src.PortionSizeId));

            CreateMap<CustomersMenuPlan, DTOBoxContentSelected>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BoxContentId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.BoxContent.Name))
                .ForMember(dest => dest.PortionSizeId, opt => opt.MapFrom(src => src.PortionSizeId));

            CreateMap<DTOBoxContentOverview, List<CustomersMenuPlan>>()
                .ConvertUsing(src => src.BoxContentSelectedList!
                .Select(boxContentSelected => new CustomersMenuPlan
                {
                    BoxContentId = boxContentSelected.Id,
                    PortionSizeId = boxContentSelected.PortionSizeId,
                    Customer = null,
                    BoxContent = null,
                    PortionSize = null
                })
                .ToList() ?? new List<CustomersMenuPlan>());

            CreateMap<List<CustomersMenuPlan>, DTOBoxContentOverview>()
                .ConvertUsing(src => new DTOBoxContentOverview
                {
                    BoxContentSelectedList = src.Select(menuPlan => new DTOBoxContentSelected
                    {
                        Id = menuPlan.BoxContentId,
                        PortionSizeId = menuPlan.PortionSizeId,
                        Name = menuPlan.BoxContent.Name
                    }).ToList(),
                    SelectInputs = new List<DTOSelectInput>()
                });

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
