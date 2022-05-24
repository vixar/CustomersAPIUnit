using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities.Person;

namespace Customers.INFRASTRUCTURE.Repository.Implementations;

public class ContactNumberRepository : RepositoryAsync<ContactNumber>, IContactNumberRepository
{
    public ContactNumberRepository(CustomerDbContext context) : base(context)
    {
    }
}