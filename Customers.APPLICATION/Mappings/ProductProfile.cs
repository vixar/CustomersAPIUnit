using Application.DTOs.Product;
using Application.Services.Product;
using Application.Services.Product.Category;
using Application.Services.Product.Image;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Types;

namespace Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductImage, ProductImageDto>()
            .ForMember(x => x.ProductImageId, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<ProductImageDto, ProductImage>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductImageId));
                
        CreateMap<PatchProductImageService, ProductImage>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductImageId));
        
        CreateMap<ProductImage, PatchProductImageService>()
            .ForMember(x => x.ProductImageId, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<ProductCategory, ProductCategoryDto>()
            .ForMember(x => x.ProductCategoryId, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<ProductCategoryDto, ProductCategory>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductCategoryId));
        
        CreateMap<PatchProductCategoryService, ProductCategory>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductCategoryId));
        
        CreateMap<ProductCategory, PatchProductCategoryService>()
            .ForMember(x => x.ProductCategoryId, opt => opt.MapFrom(src => src.Id));

        CreateMap<ProductDto, PatchProductService>().ReverseMap();
        
        CreateMap<PatchProductService, Product>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductId));
        
        CreateMap<Product, PatchProductService>()
            .ForMember(x => x.ProductId, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<PatchProductService, Product>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductId));

        CreateMap<ProductDto, Product>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(x => x.Category, opt => opt.Ignore())
            .ForMember(x => x.Images, opt => opt.MapFrom(src => src.Images!.Select(x => new ProductImage
            {
                Description = x.Description,
                Large = x.Large,
                Medium = x.Medium,
                Small = x.Small,
                Main = x.Main,
                ProductId = x.ProductId
            }) ));
        
        CreateMap<Product, ProductDto>()
            .ForMember(x => x.ProductId, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.Category, opt => opt.MapFrom(src => new ProductCategoryDto()
            {
                Description = src.Description,
                Category = src.Category.Category,
                ProductCategoryId = src.ProductCategoryId
            }))
            .ForMember(x => x.Images, opt => opt.MapFrom(src => src.Images.Select(x => new ProductImageDto()
            {
                ProductImageId = x.Id,
                Large = x.Large,
                Medium = x.Medium,
                Small = x.Small,
                Main = x.Main,
                Description = x.Description,
                ProductId = x.ProductId,
                
            })));
        
    }
    
}