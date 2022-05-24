using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Application.Services.Customer;
using Application.Validations;
using Application.Validations.Customer;
using Customers.INFRASTRUCTURE.Repository.Implementations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extentions;

public static class ServiceCollectionExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient(typeof(IReposirotyAsync<>), typeof(RepositoryAsync<>));
        services.AddTransient<IValidator<AddCustomerService>, AddCustomerServiceValidations>();
        services.AddTransient<IValidator<UpdateCustomerService>, UpdateCustomerServiceValidations>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IContactNumberRepository, ContactNumberRepository>();
    }
}