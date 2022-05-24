namespace Application.Repository.Interfaces;

public interface IReposirotyAsync<T> where T : class
{
    IQueryable<T> Entities { get; }

    Task<T> GetByIdAsync(byte id);
    Task<T> GetByIdAsync(string id);
    Task<T> GetByIdAsyncAsNotTracking(byte id);
    Task<T> GetByIdAsyncAsNotTracking(string id);

    Task<List<T>> GetAllAsync();

    Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);

    Task<T> AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> range);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);
}