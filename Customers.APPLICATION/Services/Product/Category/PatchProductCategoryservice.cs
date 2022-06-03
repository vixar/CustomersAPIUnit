using Application.DTOs.Product;
using Application.Exceptions;
using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Validations;
using AutoMapper;
using Domain.Entities.Types;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Services.Product.Category;


public class PatchProductCategoryService : ProductCategoryDto, IServiceResponse<ApiResponse.Response<object>>
{
    public JsonPatchDocument<ProductCategoryDto> PatchEntity { get; set; }

    public PatchProductCategoryService(in JsonPatchDocument<ProductCategoryDto> patchEntity, in byte productCategoryId)
    {
        PatchEntity = patchEntity;
        ProductCategoryId = productCategoryId;

    }

    public PatchProductCategoryService() { }

}

public class PatchProductCategoryServiceExecutor : IServiceRequest<PatchProductCategoryService, ApiResponse.Response<object>>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<PatchProductCategoryService> _validator;

    public PatchProductCategoryServiceExecutor( IProductCategoryRepository productCategoryRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<PatchProductCategoryService> validator)
    {
        _productCategoryRepository = productCategoryRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    // TODO: Crear la lógica agregar un nuevo cliente cliente y sus numeros de contacto

    public async Task<ApiResponse.Response<object>> Execute(
        PatchProductCategoryService productCategory)
    {

        var entity = await _productCategoryRepository.GetByIdAsync(productCategory.ProductCategoryId);
        
        if(entity == null) 
            return await ApiResponse.Response<object>
                .FailAsync(
                    $"property: {nameof(productCategory.ProductCategoryId)} = ID: {productCategory.ProductCategoryId}", "object not found"
                );
        
        var productCategoryEntity = _mapper.Map<PatchProductCategoryService>(entity);
        
        try
        {
            productCategory.PatchEntity.ApplyTo(productCategoryEntity);
            
            var (item1, item2) = ProductCategoryValidator(productCategoryEntity);

            if (!item2)
            {
                return await ApiResponse.Response<object>.FailAsync(item1, "");
            }
        }
        catch (HttpException e)
        {
            throw new HttpException(e.StatusCode, e.Message);
        }
        
        var productCategoryUpdated = _mapper.Map(productCategoryEntity, entity);
        
        await _productCategoryRepository.UpdateAsync(productCategoryUpdated);
        
        await _unitOfWork.Commit(new CancellationToken());
        
        var productCategoryMapped = _mapper.Map<ProductCategoryDto>(productCategoryUpdated);
        
        return await ApiResponse.Response<object>.SuccessAsync(productCategoryMapped, "patched");
    }

    // TODO: Aislar este proceso
    private Tuple<IEnumerable<ValidationResult>?, bool> ProductCategoryValidator(PatchProductCategoryService productCategory)
    {
        var validation = _validator.ValidateAsync(productCategory).Result;

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