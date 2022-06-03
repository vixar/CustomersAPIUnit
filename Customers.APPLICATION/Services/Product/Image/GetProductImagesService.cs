using System.Linq.Expressions;
using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Types;

namespace Application.Services.Product.Image;

public class GetProductImagesService : ProductImageDto, IServiceResponse<ApiResponse.PaginatedResponse<ProductImageDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetProductImagesService(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetProductImagesServiceExecutor : IServiceRequest<GetProductImagesService, ApiResponse.PaginatedResponse<ProductImageDto>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetProductImagesServiceExecutor(IProductImageRepository productImageRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productImageRepository = productImageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.PaginatedResponse<ProductImageDto>> Execute(
        GetProductImagesService request)
    {
        Expression<Func<ProductImage, ProductImageDto>> expression = e => new ProductImageDto
        {
            ProductImageId = e.Id,
            Large = e.Large,
            Medium = e.Medium,
            Small = e.Small,
            Main = e.Main,
            ProductId = e.ProductId,
            Description = e.Description,
            CreateAt = e.CreateAt,
            UpdateAt = e.UpdateAt,
            IsActive = e.IsActive,
            Status = e.Status
        };

        var paginatedList = await _productImageRepository
            .Entities
            .Select(expression)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        return paginatedList;
    }
    
}