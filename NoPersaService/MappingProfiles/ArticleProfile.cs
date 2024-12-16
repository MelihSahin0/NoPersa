using AutoMapper;
using NoPersaService.DTOs.Article.Answer;
using NoPersaService.DTOs.Article.RA;
using NoPersaService.Models;
using NoPersaService.Util;

namespace NoPersaService.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<DTOArticle, Article>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.DecryptId(src.Id, false)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Price) ? (double?)null : double.Parse(src.Price)))
                .ForMember(dest => dest.NewName, opt => opt.MapFrom(src => src.NewName))
                .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.NewPrice) ? (double?)null : double.Parse(src.NewPrice)))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));

            CreateMap<Article, DTOArticle>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.ToString()))
                .ForMember(dest => dest.NewName, opt => opt.MapFrom(src => src.NewName))
                .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.NewPrice.ToString()))
                .ForMember(dest => dest.NumberOfCustomers, opt => opt.MapFrom(src => src.Customers.Count))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));
        
            CreateMap<Article, DTOSelectArticleWithPrice>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdEncryption.EncryptId(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.ToString()))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));
        }
    }
}
