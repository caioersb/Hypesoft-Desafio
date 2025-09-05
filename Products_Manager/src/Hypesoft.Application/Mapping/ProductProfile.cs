using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;

namespace Hypesoft.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Mapeamento de Product → ProductReadDto
            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            // Mapeamento de ProductCreateDto → Product
            CreateMap<ProductCreateDto, Product>();

            // Mapeamento de ProductUpdateDto → Product
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
