using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Validations;
using AutoMapper;
using Domain.Entities.Types;
using FluentValidation;

namespace Application.Services.Product.Category;


public class AddProductCategoryService : ProductCategoryDto, IServiceResponse<ApiResponse.Response<object>>
{

}

public class AddProductCategoryServiceExecutor : IServiceRequest<AddProductCategoryService, ApiResponse.Response<object>>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddProductCategoryService> _validator;

    public AddProductCategoryServiceExecutor( IProductCategoryRepository productCategoryRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<AddProductCategoryService> validator)
    {
        _productCategoryRepository = productCategoryRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    // TODO: Crear la lógica agregar un nuevo cliente cliente y sus numeros de contacto

    public async Task<ApiResponse.Response<object>> Execute(
        AddProductCategoryService productCategory)
    {
        var validation = ProductCategoryValidator(productCategory);

        if (!validation.Item2)
        {
            return await ApiResponse.Response<object>.FailAsync(validation.Item1, "");
        }
        
        var productCategoryEntity = _mapper.Map<AddProductCategoryService, ProductCategory>(productCategory);
        await _productCategoryRepository.AddAsync(productCategoryEntity);
        await _unitOfWork.Commit(new CancellationToken());
        var productCategoryMapped = _mapper.Map<ProductCategoryDto>(productCategoryEntity);
        return await ApiResponse.Response<object>.SuccessAsync(productCategoryMapped, "created");
    }

    // TODO: Aislar este proceso
    private Tuple<IEnumerable<ValidationResult>?, bool> ProductCategoryValidator(AddProductCategoryService productCategory)
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