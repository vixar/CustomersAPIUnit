using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;

namespace Application.Services.Product.Category;

public class GetByIdProductCategoryService : ProductCategoryDto, IServiceResponse<ApiResponse.Response<ProductCategoryDto>>
{
    public byte ProductCategoryId { get; set; }
}

public class GetByIdProductCategoryServiceExecutor : IServiceRequest<GetByIdProductCategoryService, ApiResponse.Response<ProductCategoryDto>>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdProductCategoryServiceExecutor(IProductCategoryRepository productCategoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productCategoryRepository = productCategoryRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.Response<ProductCategoryDto>> Execute(
        GetByIdProductCategoryService request)
    {
        
        var productCategory = await _productCategoryRepository.GetByIdAsync(request.ProductCategoryId);
        
        if (productCategory == null)
        {
            return await ApiResponse.Response<ProductCategoryDto>.FailAsync($"Not Found {request.ProductCategoryId}");
        }
        
        var productCategoryMapped = _mapper.Map<ProductCategoryDto>(productCategory);

        return await ApiResponse.Response<ProductCategoryDto>.SuccessAsync(productCategoryMapped, "removed");
    }
    
}