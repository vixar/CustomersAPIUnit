using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;

namespace Application.Services.Product;

public class GetByIdProductService : ProductDto, IServiceResponse<ApiResponse.Response<ProductDto>>
{
    public string ProductId { get; set; }
}

public class GetByIdProductServiceExecutor : IServiceRequest<GetByIdProductService, ApiResponse.Response<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdProductServiceExecutor(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.Response<ProductDto>> Execute(
        GetByIdProductService request)
    {
        
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        
        if (product == null)
        {
            return await ApiResponse.Response<ProductDto>.FailAsync($"Not Found {request.ProductId}");
        }
        
        var productMapped = _mapper.Map<ProductDto>(product);

        return await ApiResponse.Response<ProductDto>.SuccessAsync(productMapped, "removed");
    }
    
}