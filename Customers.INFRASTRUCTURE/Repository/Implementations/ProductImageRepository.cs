using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Customers.INFRASTRUCTURE.Repository.Implementations;

public class ProductImageRepository : RepositoryAsync<ProductImage>, IProductImageRepository
{
    private readonly CustomerDbContext _dbContext;

    public ProductImageRepository(CustomerDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IQueryable<ProductImage>> GetProductImages(string productId)
    {
        return _dbContext.ProductImages.Where(x => x.ProductId == productId);
    }
}