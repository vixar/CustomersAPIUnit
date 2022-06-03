using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Validations;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Types;
using FluentValidation;

namespace Application.Services.Product.Image;


public class AddProductImageService : ProductImageDto, IServiceResponse<ApiResponse.Response<object>>
{

}

public class AddProductImageServiceExecutor : IServiceRequest<AddProductImageService, ApiResponse.Response<object>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddProductImageService> _validator;

    public AddProductImageServiceExecutor( IProductImageRepository productImageRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<AddProductImageService> validator)
    {
        _productImageRepository = productImageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    // TODO: Crear la lógica agregar un nuevo cliente cliente y sus numeros de contacto

    public async Task<ApiResponse.Response<object>> Execute(
        AddProductImageService productImage)
    {
        var validation = ProductImageValidator(productImage);

        if (!validation.Item2)
        {
            return await ApiResponse.Response<object>.FailAsync(validation.Item1, "");
        }
        
        var productImageEntity = _mapper.Map<AddProductImageService, ProductImage>(productImage);
        await _productImageRepository.AddAsync(productImageEntity);
        await _unitOfWork.Commit(new CancellationToken());
        var productImageMapped = _mapper.Map<ProductImageDto>(productImageEntity);
        return await ApiResponse.Response<object>.SuccessAsync(productImageMapped, "created");
    }

    // TODO: Aislar este proceso
    private Tuple<IEnumerable<ValidationResult>?, bool> ProductImageValidator(AddProductImageService productImage)
    {
        var validation = _validator.ValidateAsync(productImage).Result;

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