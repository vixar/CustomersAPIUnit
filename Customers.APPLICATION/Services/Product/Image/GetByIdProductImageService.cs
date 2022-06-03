using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;

namespace Application.Services.Product.Image;

public class GetByIdProductImageService : ProductImageDto, IServiceResponse<ApiResponse.Response<ProductImageDto>>
{
    public string ProductImageId { get; set; }
}

public class GetByIdProductImageServiceExecutor : IServiceRequest<GetByIdProductImageService, ApiResponse.Response<ProductImageDto>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdProductImageServiceExecutor(IProductImageRepository productImageRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productImageRepository = productImageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.Response<ProductImageDto>> Execute(
        GetByIdProductImageService request)
    {
        
        var productImage = await _productImageRepository.GetByIdAsync(request.ProductImageId);
        
        if (productImage == null)
        {
            return await ApiResponse.Response<ProductImageDto>.FailAsync($"Not Found {request.ProductImageId}");
        }
        
        var productImageMapped = _mapper.Map<ProductImageDto>(productImage);

        return await ApiResponse.Response<ProductImageDto>.SuccessAsync(productImageMapped, "removed");
    }
    
}