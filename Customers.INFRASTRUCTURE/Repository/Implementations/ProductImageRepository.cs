using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities.Products;

namespace Customers.INFRASTRUCTURE.Repository.Implementations;

public class ProductImageRepository : RepositoryAsync<ProductImage>, IProductImageRepository
{
    public ProductImageRepository(CustomerDbContext dbContext) : base(dbContext)
    {
    }
}