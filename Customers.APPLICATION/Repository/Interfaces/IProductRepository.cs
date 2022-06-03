using Domain.Entities.Products;

namespace Application.Repository.Interfaces;

public interface IProductRepository : IReposirotyAsync<Product>
{
    Task<List<Product>> GetProductsByCategory(byte id);    
    Task<int> GetCountByCategory(byte id);
    Task<int> GetCount();
    Task<Product> GetProductWithCategoryAndImages(string id);
    Task<List<Product>> GetProductsWithCategoryAndImages();
}