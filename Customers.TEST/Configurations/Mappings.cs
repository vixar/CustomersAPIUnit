using System.Linq;
using Application.DTOs.Customer;
using Application.DTOs.Product;
using Application.Services.Product;
using Application.Services.Product.Category;
using Application.Services.Product.Image;
using AutoMapper;
using Domain.Entities.Customers;
using Domain.Entities.Products;
using Domain.Entities.Types;

namespace Customers.TEST.Configurations;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(x => x.CustomerId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Birthday, opt => opt.MapFrom(x => x.Birthday.ToString("dd/MM/yyyy")));

        CreateMap<CustomerDto, Customer>()
            .ForMember(x => x.ContactNumbers, opt => opt.MapFrom(x => x.ContactNumbers)).ReverseMap();
        
        //---------------------------------------------------------------------------------------------------------------------
        // PRODUCTS
        //---------------------------------------------------------------------------------------------------------------------

        #region PRODUCTS_MAPPINGS
        
        CreateMap<PatchProductCategoryService, ProductCategory>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductCategoryId));
        
        CreateMap<ProductCategory, PatchProductCategoryService>()
            .ForMember(x => x.ProductCategoryId, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<PatchProductImageService, ProductImage>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductImageId));
        
        CreateMap<ProductImage, PatchProductImageService>()
            .ForMember(x => x.ProductImageId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(x => x.ProductImageId, opt => opt.MapFrom(src => src.Id));
            
            CreateMap<ProductImageDto, ProductImage>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductImageId));
            
            CreateMap<ProductCategory, ProductCategoryDto>()
                .ForMember(x => x.ProductCategoryId, opt => opt.MapFrom(src => src.Id));
            
            CreateMap<ProductCategoryDto, ProductCategory>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductCategoryId));
            
            CreateMap<ProductDto, PatchProductService>().ReverseMap();
        
            CreateMap<PatchProductService, Product>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ProductId));
            
            CreateMap<Product, PatchProductService>()
                .ForMember(x => x.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Images, opt => opt.Ignore());

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
                    ProductId = x.ProductId
            })));
        #endregion
        
    }
}