using System.ComponentModel.DataAnnotations;
using Application.DTOs.Common;
using Domain.Entities;
using Domain.Entities.Person;

namespace Application.DTOs.Customer;

public class CustomerDto : BaseEntity
{
    public string? CustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public string? Birthday { get; set; }
    public IEnumerable<ContactNumberDto>? ContactNumbers { get; set; }
}