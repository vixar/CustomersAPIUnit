using Domain.Entities.Customers;
using Domain.Entities.Person;

namespace Application.Repository.Interfaces;

public interface ICustomerRepository : IReposirotyAsync<Customer>
{
    Task<Customer?> GetByIdWithContactNumbers(string customerId);
}