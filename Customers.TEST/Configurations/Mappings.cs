using Application.DTOs.Customer;
using AutoMapper;
using Domain.Entities.Customers;

namespace Customers.TEST.Configurations;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(x => x.CustomerId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Birthday, opt => opt.MapFrom(x => x.Birthday.ToString("dd/MM/yyyy")));

        CreateMap<CustomerDto, Customer>()
            .ForMember(x => x.ContactNumbers, opt => opt.MapFrom(x => x.ContactNumbers)).ReverseMap();
    }
}