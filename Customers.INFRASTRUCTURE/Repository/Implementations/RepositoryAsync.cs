using Application.Exceptions;
using Application.Repository.Interfaces;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.Implementations;

public class RepositoryAsync<T> : IReposirotyAsync<T> where T : class
{
    private readonly CustomerDbContext _context;

        public RepositoryAsync(CustomerDbContext dbContext)
        {
            _context = dbContext;
        }

        public virtual IQueryable<T> Entities => _context.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> AddAsyncAndSave(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            // await _context.SaveChangesAsync(new System.Threading.CancellationToken());

            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> range)
        {
            await _context.Set<IEnumerable<T>>().AddRangeAsync(range);

        }

        public Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async virtual Task<List<T>> GetAllAsync()
        {
            return await _context
                .Set<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(byte id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        
        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }


        public async Task<T> GetByIdAsyncAsNotTracking(string id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity == null) throw new HttpException(System.Net.HttpStatusCode.BadRequest, $"{typeof(T).Name} null; parameter: {nameof(id)}=> {id}; ");
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        
        public async Task<T> GetByIdAsyncAsNotTracking(byte id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity == null) throw new HttpException(System.Net.HttpStatusCode.BadRequest, $"{typeof(T).Name} null; parameter: {nameof(id)}=> {id}; ");
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _context
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task UpdateAsync(T entity)
        {
            // _context.Entry(entity).CurrentValues.SetValues(entity);
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }
}