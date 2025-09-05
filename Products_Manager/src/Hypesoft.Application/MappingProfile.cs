using AutoMapper;
using Hypesoft.Domain.Entities;
using Hypesoft.Application.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();

        // Category
        CreateMap<Category, CategoryReadDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
    }
}
