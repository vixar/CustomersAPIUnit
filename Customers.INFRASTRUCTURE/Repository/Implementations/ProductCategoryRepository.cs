using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities.Types;

namespace Customers.INFRASTRUCTURE.Repository.Implementations;

public class ProductCategoryRepository : RepositoryAsync<ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(CustomerDbContext dbContext) : base(dbContext)
    {
    }
}