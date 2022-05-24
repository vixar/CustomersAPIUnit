using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;

namespace Customers.INFRASTRUCTURE.Repository.Implementations;

public class CustomerRepository : RepositoryAsync<Customer>, ICustomerRepository
{
    private readonly CustomerDbContext _context;
    public CustomerRepository(CustomerDbContext context) : base(context)
    {
        _context = context;
    }
    
    
    public async Task<Customer?> GetByIdWithContactNumbers(string customerId)
    {
        return await _context.Customer
            .Include(x => x.ContactNumbers)
            .ThenInclude(x => x.ContactNumberType)
            .FirstOrDefaultAsync(x => x.Id == customerId);
    }

}