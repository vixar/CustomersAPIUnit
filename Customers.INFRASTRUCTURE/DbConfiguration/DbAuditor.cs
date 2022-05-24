using Application.Exceptions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customers.INFRASTRUCTURE.DbConfiguration;

public abstract class DbAuditor : DbContext
{
    public DbAuditor(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Auditory> Auditories { get; set; }

    private async Task<int> SaveAsync()
    {
        try
        {
            return await base.SaveChangesAsync(new CancellationToken());
        }
        catch (Exception e)
        {

            Console.WriteLine(e);
            throw new HttpException(System.Net.HttpStatusCode.BadRequest,
                e.InnerException != null ? e.InnerException.Message : e.Message);
            // TODO: Log error
        }
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        // TODO: Crear proceso para auditoria
        // ISSUE: reference to a compiler-generated method
        int result = await this.SaveAsync();
        int num = result;
        return num;
    }
}