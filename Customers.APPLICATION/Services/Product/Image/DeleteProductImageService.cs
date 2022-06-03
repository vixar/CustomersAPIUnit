using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;

namespace Application.Services.Product.Image;

public class DeleteProductImageService : ProductImageDto, IServiceResponse<ApiResponse.Response<object>>
{
    public string ProductImageId { get; set; }
}

public class DeleteProductImageServiceExecutor : IServiceRequest<DeleteProductImageService, ApiResponse.Response<object>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductImageServiceExecutor(IProductImageRepository productImageRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productImageRepository = productImageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.Response<object>> Execute(
        DeleteProductImageService request)
    {
        
        var productImage = await _productImageRepository.GetByIdAsync(request.ProductImageId);
        
        if (productImage == null)
        {
            return await ApiResponse.Response<object>.FailAsync($"Not Found {request.ProductImageId}");
        }
        
        await _productImageRepository.DeleteAsync(productImage);
        
        await _unitOfWork.Commit(new CancellationToken());
        
        return await ApiResponse.Response<object>.SuccessAsync(request.ProductImageId, "removed");
    }
    
}