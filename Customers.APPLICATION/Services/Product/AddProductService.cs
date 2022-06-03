using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Validations;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Entities.Types;
using FluentValidation;

namespace Application.Services.Product;


public class AddProductService : ProductDto, IServiceResponse<ApiResponse.Response<object>>
{

}

public class AddProductServiceExecutor : IServiceRequest<AddProductService, ApiResponse.Response<object>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddProductService> _validator;

    public AddProductServiceExecutor( IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<AddProductService> validator)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    // TODO: Crear la lógica agregar un nuevo cliente cliente y sus numeros de contacto

    public async Task<ApiResponse.Response<object>> Execute(
        AddProductService product)
    {
        var (item1, item2) = await ServiceValidator.ServiceValidationAsync<AddProductService>(product, _validator);

        if (!item2)
        {
            return await ApiResponse.Response<object>.FailAsync(item1, "");
        }
        
        var productEntity = _mapper.Map<AddProductService, Domain.Entities.Products.Product>(product);
        
        await _productRepository.AddAsync(productEntity);
        await _unitOfWork.Commit(new CancellationToken());
        var productMapped = _mapper.Map<ProductDto>(productEntity);
        return await ApiResponse.Response<object>.SuccessAsync(productMapped, "created");
    }

    // TODO: Aislar este proceso
    // private async Task<Tuple<IEnumerable<ValidationResult>?, bool>> ProductValidator(AddProductService product)
    // {
    //     
    //     var validation = await _validator.ValidateAsync(product);
    //     
    //     return validation.IsValid 
    //         ? Tuple.Create<IEnumerable<ValidationResult>?, bool>(null, true) 
    //         : Tuple.Create(validation.Errors?.Select(e => new ValidationResult(
    //             e.ErrorCode,
    //             e.PropertyName,
    //             e.ErrorMessage
    //             )), false);
    // }
}