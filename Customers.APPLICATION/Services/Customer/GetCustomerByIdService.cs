using Application.DTOs.Customer;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;

namespace Application.Services.Customer;

public class GetCustomerByIdService : CustomerDto, IServiceResponse<ApiResponse.Response<object>>
{
    public string CustomerId { get; set; }
}

public class GetCustomerByIdServiceExecutor : IServiceRequest<GetCustomerByIdService, ApiResponse.Response<object>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerByIdServiceExecutor(ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.Response<object>> Execute(
        GetCustomerByIdService request)
    {
        
        var customer = await _customerRepository.GetByIdWithContactNumbers(request.CustomerId);

        var customerMapped = _mapper.Map<CustomerDto>(customer);
        
        return await ApiResponse.Response<object>.SuccessAsync(customerMapped, "");
    }
    
}