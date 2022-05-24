using System.Globalization;
using Application.DTOs.Customer;
using AutoMapper;
using Domain.Entities.Customers;

namespace Application.Mappings;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(x => x.CustomerId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Birthday, opt => 
                opt.MapFrom(x => x.Birthday.Date.ToString("dd-MM-yyyy", new CultureInfo("es-DO"))));

        CreateMap<CustomerDto, Customer>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.CustomerId))
            .ForMember(x => x.Birthday, opt => opt.MapFrom(x => x.Birthday))
            .ForMember(x => x.ContactNumbers, opt => opt.MapFrom(x => 
                new CustomerDto
                {
                    Birthday = x.Birthday,
                    ContactNumbers = x.ContactNumbers,
                    CustomerId = x.CustomerId,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Gender = x.Gender,
                    Status = x.Status,
                    CreateAt = x.CreateAt,
                    IsActive = x.IsActive,
                    UpdateAt = x.UpdateAt
                    
                }
                ));
    }
}