using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Customers;
using Domain.Entities.Types;

namespace Domain.Entities.Person;

public class ContactNumber : BaseEntityGuid
{
    // Understanding the customers live in Dominican Republic
    [Required, MaxLength(11)]
    public string Number { get; set; }
    public string CustomerId { get; set; }
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; }
    public byte ContactNumberTypeId { get; set; }
    [ForeignKey(nameof(ContactNumberTypeId))]
    public virtual ContactNumberType ContactNumberType { get; set; }
}