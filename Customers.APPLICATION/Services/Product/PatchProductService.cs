using Application.DTOs.Product;
using Application.Exceptions;
using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Validations;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Services.Product;


public class PatchProductService : ProductDto, IServiceResponse<ApiResponse.Response<object>>
{
    public JsonPatchDocument<ProductDto> PatchEntity { get; set; }

    public PatchProductService(in JsonPatchDocument<ProductDto> patchEntity, in string productId)
    {
        PatchEntity = patchEntity;
        ProductId = productId;

    }

    public PatchProductService() { }

}

public class PatchProductServiceExecutor : IServiceRequest<PatchProductService, ApiResponse.Response<object>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<PatchProductService> _validator;

    public PatchProductServiceExecutor( IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<PatchProductService> validator)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    // TODO: Crear la lógica agregar un nuevo cliente cliente y sus numeros de contacto

    public async Task<ApiResponse.Response<object>> Execute(
        PatchProductService product)
    {

        var entity = await _productRepository.GetByIdAsync(product.ProductId);
        
        if(entity == null) 
            return await ApiResponse.Response<object>
                .FailAsync(
                    $"property: {nameof(product.ProductId)} = ID: {product.ProductId}", "object not found"
                );
        
        var productEntity = _mapper.Map<PatchProductService>(entity);
        
        try
        {
            product.PatchEntity.ApplyTo(productEntity);
            
            var (item1, item2) = await ServiceValidator.ServiceValidationAsync<PatchProductService>(productEntity, _validator);

            if (!item2)
            {
                return await ApiResponse.Response<object>.FailAsync(item1, "");
            }
        }
        catch (HttpException e)
        {
            throw new HttpException(e.StatusCode, e.Message);
        }
        
        var productUpdated = _mapper.Map(productEntity, entity);
        
        await _productRepository.UpdateAsync(productUpdated);
        
        await _unitOfWork.Commit(new CancellationToken());
        
        var productMapped = _mapper.Map<ProductDto>(productUpdated);
        
        return await ApiResponse.Response<object>.SuccessAsync(productMapped, "patched");
    }

    
}