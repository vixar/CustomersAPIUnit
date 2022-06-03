using Application.DTOs.Product;
using Application.Exceptions;
using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Validations;
using AutoMapper;
using Domain.Entities.Types;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Services.Product.Image;


public class PatchProductImageService : ProductImageDto, IServiceResponse<ApiResponse.Response<object>>
{
    public JsonPatchDocument<ProductImageDto> PatchEntity { get; set; }

    public PatchProductImageService(in JsonPatchDocument<ProductImageDto> patchEntity, in string productImageId)
    {
        PatchEntity = patchEntity;
        ProductImageId = productImageId;

    }

    public PatchProductImageService() { }

}

public class PatchProductImageServiceExecutor : IServiceRequest<PatchProductImageService, ApiResponse.Response<object>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<PatchProductImageService> _validator;

    public PatchProductImageServiceExecutor( IProductImageRepository productImageRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<PatchProductImageService> validator)
    {
        _productImageRepository = productImageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    // TODO: Crear la lógica agregar un nuevo cliente cliente y sus numeros de contacto

    public async Task<ApiResponse.Response<object>> Execute(
        PatchProductImageService productImage)
    {

        var entity = await _productImageRepository.GetByIdAsync(productImage.ProductImageId);
        
        if(entity == null) 
            return await ApiResponse.Response<object>
                .FailAsync(
                    $"property: {nameof(productImage.ProductImageId)} = ID: {productImage.ProductImageId}", "object not found"
                );
        
        var productImageEntity = _mapper.Map<PatchProductImageService>(entity);
        
        try
        {
            productImage.PatchEntity.ApplyTo(productImageEntity);
            
            var (item1, item2) = ProductImageValidator(productImageEntity);

            if (!item2)
            {
                return await ApiResponse.Response<object>.FailAsync(item1, "");
            }
        }
        catch (HttpException e)
        {
            throw new HttpException(e.StatusCode, e.Message);
        }
        
        var productImageUpdated = _mapper.Map(productImageEntity, entity);
        
        await _productImageRepository.UpdateAsync(productImageUpdated);
        
        await _unitOfWork.Commit(new CancellationToken());
        
        var productImageMapped = _mapper.Map<ProductImageDto>(productImageUpdated);
        
        return await ApiResponse.Response<object>.SuccessAsync(productImageMapped, "patched");
    }

    // TODO: Aislar este proceso
    private Tuple<IEnumerable<ValidationResult>?, bool> ProductImageValidator(PatchProductImageService productImage)
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