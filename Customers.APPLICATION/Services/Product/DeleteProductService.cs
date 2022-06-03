using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;

namespace Application.Services.Product;

public class DeleteProductService : ProductDto, IServiceResponse<ApiResponse.Response<object>>
{
    public string ProductId { get; set; }
}

public class DeleteProductServiceExecutor : IServiceRequest<DeleteProductService, ApiResponse.Response<object>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductServiceExecutor(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.Response<object>> Execute(
        DeleteProductService request)
    {
        
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        
        if (product == null)
        {
            return await ApiResponse.Response<object>.FailAsync($"Not Found {request.ProductId}");
        }
        
        await _productRepository.DeleteAsync(product);
        
        await _unitOfWork.Commit(new CancellationToken());
        
        return await ApiResponse.Response<object>.SuccessAsync(request.ProductId, "removed");
    }
    
}