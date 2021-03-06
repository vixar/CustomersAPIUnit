using Domain.Entities.Products;

namespace Application.Repository.Interfaces;

public interface IProductImageRepository : IReposirotyAsync<ProductImage>
{
    Task<IQueryable<ProductImage>> GetProductImages(string productId);
}