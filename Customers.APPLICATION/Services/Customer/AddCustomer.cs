using Application.DTOs.Customer;
using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Validations;
using AutoMapper;
using FluentValidation;

namespace Application.Services.Customer;

public class AddCustomerService : CustomerDto, IServiceResponse<ApiResponse.Response<object>>
{

}

public class AddCustomerServiceExecutor : IServiceRequest<AddCustomerService, ApiResponse.Response<object>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddCustomerService> _validator;

    public AddCustomerServiceExecutor( ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<AddCustomerService> validator)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    // TODO: Crear la lógica agregar un nuevo cliente cliente y sus numeros de contacto

    public async Task<ApiResponse.Response<object>> Execute(
        AddCustomerService customer)
    {
        var validation = CustomerValidator(customer);

        if (!validation.Item2)
        {
            return await ApiResponse.Response<object>.FailAsync(validation.Item1, "");
        }
        
        var customerEntity = _mapper.Map<AddCustomerService, Domain.Entities.Customers.Customer>(customer);
        await _customerRepository.AddAsync(customerEntity);
        await _unitOfWork.Commit(new CancellationToken());
        var customerMapped = _mapper.Map<CustomerDto>(customerEntity);
        customerMapped.ContactNumbers = null;
        return await ApiResponse.Response<object>.SuccessAsync(customerMapped, "created");
    }

    // TODO: Aislar este proceso
    private Tuple<IEnumerable<ValidationResult>?, bool> CustomerValidator(AddCustomerService customer)
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


