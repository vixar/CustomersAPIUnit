using System.Linq;
using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Customers.INFRASTRUCTURE.Repository.Implementations;

public class ProductRepository : RepositoryAsync<Product>, IProductRepository
{
    private readonly CustomerDbContext _dbContext;

    public ProductRepository(CustomerDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetProductsByCategory(byte id)
    {
        return await _dbContext.Products.Where(x => x.ProductCategoryId == id).ToListAsync();
    }

    public async Task<int> GetCountByCategory(byte id)
    {
        return await _dbContext.Products.Where(x => x.ProductCategoryId == id).CountAsync();
    }

    public async Task<int> GetCount()
    {
        return await _dbContext.Products.CountAsync();
    }

    public async Task<Product> GetProductWithCategoryAndImages(string id)
    {
        return await _dbContext.Products.Include(x => x.Category).Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Product>> GetProductsWithCategoryAndImages()
    { 
        return await _dbContext.Products.Include(x => x.Category).Include(x => x.Images).ToListAsync();
    }
}