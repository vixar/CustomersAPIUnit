using Application.Repository.Interfaces;
using Cutomers.INFRASTRUCTURE.DbConfiguration;

namespace Application.Repository.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly CustomerDbContext _context;
    private bool disposed;
    
    public UnitOfWork(CustomerDbContext context)
    {
        if(context == null)
            throw new ArgumentNullException(nameof(context));

        _context = context;
    }

    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public Task Rollback()
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                //dispose managed resources
                _context.Dispose();
            }
        }
        //dispose unmanaged resources
        disposed = true;
    }
}