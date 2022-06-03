using System.Linq.Expressions;
using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;
using Domain.Entities.Types;

namespace Application.Services.Product.Category;

public class GetProductCategoriesService : ProductCategoryDto, IServiceResponse<ApiResponse.PaginatedResponse<ProductCategoryDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetProductCategoriesService(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetProductCategoriesServiceExecutor : IServiceRequest<GetProductCategoriesService, ApiResponse.PaginatedResponse<ProductCategoryDto>>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetProductCategoriesServiceExecutor(IProductCategoryRepository productCategoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productCategoryRepository = productCategoryRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.PaginatedResponse<ProductCategoryDto>> Execute(
        GetProductCategoriesService request)
    {
        Expression<Func<ProductCategory, ProductCategoryDto>> expression = e => new ProductCategoryDto
        {
            ProductCategoryId = e.Id,
            Category = e.Category,
            Description = e.Description,
            CreateAt = e.CreateAt,
            UpdateAt = e.UpdateAt,
            IsActive = e.IsActive,
            Status = e.Status
        };

        var paginatedList = await _productCategoryRepository
            .Entities
            .Select(expression)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        return paginatedList;
    }
    
}