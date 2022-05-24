using Application.DTOs.Customer;
using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Validations;
using AutoMapper;
using FluentValidation;

namespace Application.Services.Customer;

public class UpdateCustomerService : CustomerDto, IServiceResponse<ApiResponse.Response<object>>
{
    public string? Id { get; set; }
}

public class UpdateCustomerServiceExecutor : IServiceRequest<UpdateCustomerService, ApiResponse.Response<object>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateCustomerService> _validator;

    public UpdateCustomerServiceExecutor( ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<UpdateCustomerService> validator)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    // TODO: Crear la lógica agregar un nuevo cliente cliente y sus numeros de contacto

    public async Task<ApiResponse.Response<object>> Execute(
        UpdateCustomerService customer)
    {
        var validation = CustomerValidator(customer);

        if (!validation.Item2)
        {
            return await ApiResponse.Response<object>.FailAsync(validation.Item1, "");
        }
        
        // validar si el cliente existe
        await _customerRepository.GetByIdAsyncAsNotTracking(customer.CustomerId);

        var customerEntity = _mapper.Map<UpdateCustomerService, Domain.Entities.Customers.Customer>(customer);

        await _customerRepository.UpdateAsync(customerEntity);
        
        await _unitOfWork.Commit(new CancellationToken());
        
        var customerMapped = _mapper.Map<CustomerDto>(customerEntity);
        
        return await ApiResponse.Response<object>.SuccessAsync(customerMapped, "updated");
    }
    
    // TODO: Aislar este proceso

    private Tuple<IEnumerable<ValidationResult>?, bool> CustomerValidator(UpdateCustomerService customer)
    {
        var validation = _validator.ValidateAsync(customer).Result;

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


