using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Validations;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Types;
using FluentValidation;

namespace Application.Services.Product.Image;


public class UpdateProductImageService : ProductImageDto, IServiceResponse<ApiResponse.Response<object>>
{
}

public class UpdateProductImageServiceExecutor : IServiceRequest<UpdateProductImageService, ApiResponse.Response<object>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateProductImageService> _validator;

    public UpdateProductImageServiceExecutor( IProductImageRepository productImageRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<UpdateProductImageService> validator)
    {
        _productImageRepository = productImageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    // TODO: Crear la lógica agregar un nuevo cliente cliente y sus numeros de contacto

    public async Task<ApiResponse.Response<object>> Execute(
        UpdateProductImageService productImage)
    {
        var (item1, item2) = await ProductImageValidator(productImage);

        if (!item2)
        {
            return await ApiResponse.Response<object>.FailAsync(item1, "");
        }

        await _productImageRepository.GetByIdAsyncAsNotTracking(productImage.ProductImageId);
        
        var productImageEntity = _mapper.Map<UpdateProductImageService, ProductImage>(productImage);
        await _productImageRepository.UpdateAsync(productImageEntity);
        await _unitOfWork.Commit(new CancellationToken());
        var productImageMapped = _mapper.Map<ProductImageDto>(productImageEntity);
        return await ApiResponse.Response<object>.SuccessAsync(productImageMapped, "updated");
    }

    // TODO: Aislar este proceso
    private async Task<Tuple<IEnumerable<ValidationResult>?, bool>> ProductImageValidator(UpdateProductImageService productImage)
    {
        var validation = await _validator.ValidateAsync(productImage);

        // TODO: Convertir este proceso en un atributo
        if (!validation.IsValid)
        {
            return Tuple.Create(
                validation.Errors?.Select(e =>
                    new ValidationResult(
                        e.ErrorCode,
                        e.PropertyName,
                        e.ErrorMessage
                    )),
                validation.IsValid
            );

        }
        
        return Tuple.Create<IEnumerable<ValidationResult>?, bool>(null, validation.IsValid);
    }
}