using System.Globalization;
using System.Linq.Expressions;
using Application.DTOs.Common;
using Application.DTOs.Customer;
using Application.Extentions;
using Application.Repository.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Customer;

public class GetCustomerListPagedService : CustomerDto, IServiceResponse<ApiResponse.PaginatedResponse<CustomerDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetCustomerListPagedService(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetCustomerListPagedServiceExecutor : IServiceRequest<GetCustomerListPagedService, ApiResponse.PaginatedResponse<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerListPagedServiceExecutor(ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    // TODO: Crear la lógica para retornar un cliente por id
    
    public async Task<ApiResponse.PaginatedResponse<CustomerDto>> Execute(
        GetCustomerListPagedService request)
    {
        Expression<Func<Domain.Entities.Customers.Customer, CustomerDto>> expression = e =>
            new CustomerDto
            {
                CustomerId = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Gender = e.Gender,
                IsActive = e.IsActive,
                Status = e.Status ,
                CreateAt = e.CreateAt,
                UpdateAt = e.UpdateAt,
                Birthday = e.Birthday.ToString("dd-MM-yyyy", new CultureInfo("es-DO")),
                ContactNumbers = e.ContactNumbers.Select(x => new ContactNumberDto
                {
                    Number = x.Number,
                    ContactNumberTypeDescription = x.ContactNumberType.Type,
                    ContactNumberTypeId = x.ContactNumberTypeId
                })
            };
        var paginatedList = await _customerRepository.Entities
            .Include(x => x.ContactNumbers)
            .ThenInclude(x => x.ContactNumberType)
            .Select(expression)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        return paginatedList;
    }
    
}