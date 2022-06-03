using System.Linq.Expressions;
using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Types;
using Newtonsoft.Json;

namespace Application.Services.Product.Image;

public class GetProductImagesByProductIdService : ProductImageDto, IServiceResponse<ApiResponse.PaginatedResponse<ProductImageDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string ProductId { get; set; }

    public GetProductImagesByProductIdService(string productId, int pageNumber, int pageSize)
    {
        ProductId = productId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetProductImagesByProductIdServiceExecutor : IServiceRequest<GetProductImagesByProductIdService, ApiResponse.PaginatedResponse<ProductImageDto>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetProductImagesByProductIdServiceExecutor(IProductImageRepository productImageRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productImageRepository = productImageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.PaginatedResponse<ProductImageDto>> Execute(
        GetProductImagesByProductIdService request)
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

        var paginatedList = 
            await ( await _productImageRepository.GetProductImages(request.ProductId))
                .Select(expression)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        return paginatedList;
    }
    
}