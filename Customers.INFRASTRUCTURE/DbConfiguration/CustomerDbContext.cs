using Application.Exceptions;
using Customers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities;
using Domain.Entities.Customers;
using Domain.Entities.Person;
using Microsoft.EntityFrameworkCore;

namespace Cutomers.INFRASTRUCTURE.DbConfiguration;

public partial class CustomerDbContext : DbAuditor
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreateAt = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateAt = DateTime.Now;
                    break;
            }
        }

        try
        {
            return await base.SaveChangesAsync();
        }
        catch (Exception e)
        {
                
            Console.WriteLine(e);
            throw new HttpException(System.Net.HttpStatusCode.BadRequest, 
                e.InnerException != null ? e.InnerException.Message : e.Message);
            // TODO: Log error
        }
    }   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactNumber>(
                entity =>
                {
                    entity.HasIndex(c => c.Number).IsUnique();
                    entity.HasIndex(c => new {c.ContactNumberTypeId, c.CustomerId}).IsUnique();
                });
        modelBuilder.Entity<Customer>(
            entity =>
            {
                entity.HasIndex(c => new {c.FirstName, c.LastName}).IsUnique();
                entity.HasIndex(c => c.Email).IsUnique();
            });
        base.OnModelCreating(modelBuilder);
    }
    //public DbSet<Customer> Customers { get; set; }
}