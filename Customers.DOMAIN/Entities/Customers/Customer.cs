using Domain.Entities.Person;

namespace Domain.Entities.Customers;

public class Customer : PersonBaseEntity<string>
{
    public virtual ICollection<ContactNumber> ContactNumbers { get; set; }
}