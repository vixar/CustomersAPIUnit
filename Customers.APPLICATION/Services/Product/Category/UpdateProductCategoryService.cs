using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Validations;
using AutoMapper;
using Domain.Entities.Types;
using FluentValidation;

namespace Application.Services.Product.Category;


public class UpdateProductCategoryService : ProductCategoryDto, IServiceResponse<ApiResponse.Response<object>>
{
    
    //public byte ProductCategoryId { get; set; }

    public UpdateProductCategoryService(in UpdateProductCategoryService productCategoryDto, in byte productCategoryId)
    {
        ProductCategoryId = productCategoryId;
        Category = productCategoryDto.Category;
        Description = productCategoryDto.Description;
    }

    public UpdateProductCategoryService()
    {
        
    }

}

public class UpdateProductCategoryServiceExecutor : IServiceRequest<UpdateProductCategoryService, ApiResponse.Response<object>>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateProductCategoryService> _validator;

    public UpdateProductCategoryServiceExecutor( IProductCategoryRepository productCategoryRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<UpdateProductCategoryService> validator)
    {
        _productCategoryRepository = productCategoryRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    // TODO: Crear la lógica agregar un nuevo cliente cliente y sus numeros de contacto

    public async Task<ApiResponse.Response<object>> Execute(
        UpdateProductCategoryService productCategory)
    {
        var (item1, item2) = ProductCategoryValidator(productCategory);

        if (!item2)
        {
            return await ApiResponse.Response<object>.FailAsync(item1, "");
        }

        await _productCategoryRepository.GetByIdAsyncAsNotTracking(productCategory.ProductCategoryId);
        
        var productCategoryEntity = _mapper.Map<UpdateProductCategoryService, ProductCategory>(productCategory);
        await _productCategoryRepository.UpdateAsync(productCategoryEntity);
        await _unitOfWork.Commit(new CancellationToken());
        var productCategoryMapped = _mapper.Map<ProductCategoryDto>(productCategoryEntity);
        return await ApiResponse.Response<object>.SuccessAsync(productCategoryMapped, "updated");
    }

    // TODO: Aislar este proceso
    private Tuple<IEnumerable<ValidationResult>?, bool> ProductCategoryValidator(UpdateProductCategoryService productCategory)
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