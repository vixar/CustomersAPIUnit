using Domain.Entities.Customers;
using Domain.Entities.Person;
using Domain.Entities.Types;
using Microsoft.EntityFrameworkCore;

namespace Cutomers.INFRASTRUCTURE.DbConfiguration;

public partial class CustomerDbContext
{
    public virtual DbSet<Customer> Customer { get; set; }
    public virtual DbSet<ContactNumber> ContactNumbers { get; set; }
    public virtual DbSet<ContactNumberType> ContactNumberTypes { get; set; }
}