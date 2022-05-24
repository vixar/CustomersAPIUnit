using Application.Extentions;
using Application.Repository.Interfaces;
using Application.Services.Customer;
using Application.Validations;
using Application.Validations.Customer;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddCustomerService> _addValidator;
    private readonly IValidator<UpdateCustomerService> _updateValidator;

    public CustomerController(ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<AddCustomerService> addValidator, IValidator<UpdateCustomerService> updateValidator)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }
    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber, int pageSize)
    {

        var executor = new GetCustomerListPagedServiceExecutor(_customerRepository, _mapper, _unitOfWork);
        var result = await executor.Execute(new GetCustomerListPagedService(pageNumber = pageNumber, pageSize = pageSize));
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {

        var executor = new GetCustomerByIdServiceExecutor(_customerRepository, _mapper, _unitOfWork);
        var result = await executor.Execute(new GetCustomerByIdService(){CustomerId = id});
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Post(AddCustomerService customer)
    {

        var executor = new AddCustomerServiceExecutor(_customerRepository, _mapper, _unitOfWork, _addValidator);
        var result = await executor.Execute(customer);
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, UpdateCustomerService customer)
    {
        var executor = new UpdateCustomerServiceExecutor(_customerRepository, _mapper, _unitOfWork, _updateValidator);
        customer.CustomerId = id;
        var result = await executor.Execute(customer);
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var executor = new DeleteCustomerServiceExecutor(_customerRepository, _mapper, _unitOfWork);
        var result = await executor.Execute(new DeleteCustomerService{CustomerId = id});
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

}