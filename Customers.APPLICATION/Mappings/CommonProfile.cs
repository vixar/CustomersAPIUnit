using Application.DTOs.Common;
using Application.DTOs.Customer;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities.Customers;
using Domain.Entities.Person;

namespace Application.Mappings;

public class CommonProfile : Profile
{
    public CommonProfile()
    {

        CreateMap<ContactNumber, ContactNumberDto>()
            .ForMember(x => x.ContactNumberId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.ContactNumberTypeDescription, c => c.MapFrom(x => x.ContactNumberType.Type));
        
        
        CreateMap<ContactNumberDto, ContactNumber>()
            .ForMember(x => x.Id, opt => 
                opt.MapFrom(x => x.ContactNumberId ?? Guid.NewGuid().ToString()));

        CreateMap<string, DateTime>().ConvertUsing(new DateTimeTypeConverter());
    }
}