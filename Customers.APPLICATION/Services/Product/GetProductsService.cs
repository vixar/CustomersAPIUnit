using System.Linq.Expressions;
using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Types;

namespace Application.Services.Product;

public class GetProductsService : ProductDto, IServiceResponse<ApiResponse.PaginatedResponse<ProductDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetProductsService(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetProductsServiceExecutor : IServiceRequest<GetProductsService, ApiResponse.PaginatedResponse<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetProductsServiceExecutor(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.PaginatedResponse<ProductDto>> Execute(
        GetProductsService request)
    {
        Expression<Func<Domain.Entities.Products.Product, ProductDto>> expression = e => new ProductDto
        {
            ProductId = e.Id,
            Category = new ProductCategoryDto()
            {
                Category = e.Category.Category,
                ProductCategoryId = e.Category.Id,
                Description = e.Category.Description
                
            },
            Price = e.Price,
            Name = e.Name,
            Quantity = e.Quantity,
            Images = e.Images.Select(i => new ProductImageDto()
            {
                ProductImageId = i.Id,
                Description = i.Description,
                Large = i.Large,
                Medium = i.Medium,
                Small = i.Small,
                Main = i.Main,
                Status = i.Status,
                CreateAt = i.CreateAt,
                IsActive = i.IsActive,
                ProductId = i.ProductId,
                UpdateAt = i.UpdateAt
            }).ToList(),
            Description = e.Description,
            ProductCategoryId = e.ProductCategoryId,
            CreateAt = e.CreateAt,
            UpdateAt = e.UpdateAt,
            IsActive = e.IsActive,
            Status = e.Status
        };

        var paginatedList = await _productRepository
            .Entities
            .Select(expression)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        return paginatedList;
    }
    
}