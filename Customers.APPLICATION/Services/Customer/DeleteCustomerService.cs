using Application.DTOs.Customer;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;

namespace Application.Services.Customer;

public class DeleteCustomerService : CustomerDto, IServiceResponse<ApiResponse.Response<object>>
{
    public string CustomerId { get; set; }
}

public class DeleteCustomerServiceExecutor : IServiceRequest<DeleteCustomerService, ApiResponse.Response<object>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerServiceExecutor(ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.Response<object>> Execute(
        DeleteCustomerService request)
    {
        
        var customer = await _customerRepository.GetByIdWithContactNumbers(request.CustomerId);
        
        if (customer == null)
        {
            return await ApiResponse.Response<object>.FailAsync($"Not Found {request.CustomerId}");
        }
        
        await _customerRepository.DeleteAsync(customer);
        
        await _unitOfWork.Commit(new CancellationToken());
        
        return await ApiResponse.Response<object>.SuccessAsync(request.CustomerId, "removed");
    }
    
}