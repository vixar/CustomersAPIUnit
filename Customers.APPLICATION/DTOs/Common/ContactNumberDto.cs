using Domain.Entities;

namespace Application.DTOs.Common;

public class ContactNumberDto : BaseEntity
{
    public string? ContactNumberId { get; set; }
    public string Number { get; set; }
    public string? CustomerId { get; set; }
    public byte ContactNumberTypeId { get; set; }
    public string? ContactNumberTypeDescription { get; set; }
}