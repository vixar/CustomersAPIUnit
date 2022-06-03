using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;

namespace Application.Services.Product.Category;

public class DeleteProductCategoryService : ProductCategoryDto, IServiceResponse<ApiResponse.Response<object>>
{
    public byte ProductCategoryId { get; set; }
}

public class DeleteProductCategoryServiceExecutor : IServiceRequest<DeleteProductCategoryService, ApiResponse.Response<object>>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCategoryServiceExecutor(IProductCategoryRepository productCategoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productCategoryRepository = productCategoryRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.Response<object>> Execute(
        DeleteProductCategoryService request)
    {
        
        var productCategory = await _productCategoryRepository.GetByIdAsync(request.ProductCategoryId);
        
        if (productCategory == null)
        {
            return await ApiResponse.Response<object>.FailAsync($"Not Found {request.ProductCategoryId}");
        }
        
        await _productCategoryRepository.DeleteAsync(productCategory);
        
        await _unitOfWork.Commit(new CancellationToken());
        
        return await ApiResponse.Response<object>.SuccessAsync(request.ProductCategoryId, "removed");
    }
    
}