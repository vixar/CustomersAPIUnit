using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository.Implementations;
using Application.Services.Customer;
using AutoFixture;
using AutoMapper;
using Customers.INFRASTRUCTURE.Repository.Implementations;
using Customers.TEST.Configurations;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.CustomerTests;

[TestFixture]
public class AddCustomerNUnitTest
{
    private static readonly Fixture Fixture = new();
    private CustomerRepository CustomerRepository;
    private AddCustomerServiceExecutor _executor;
    private static string PrimaryKeyId = Guid.NewGuid().ToString();

    [SetUp]
    public async Task Setup()
    {
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => Fixture.Behaviors.Remove(b));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var customers = Fixture.CreateMany<Customer>().ToList();
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: $"CustomerDbContext-{Guid.NewGuid()}")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;

        var customerDbContext = new CustomerDbContext(options);

        await customerDbContext.Customer.AddRangeAsync(customers);

        await customerDbContext.SaveChangesAsync();
        
        var mapConfig = new MapperConfiguration(c =>
        {
            c.AddProfile(new Mappings());
        });

        var mapper = mapConfig.CreateMapper();

        var unitOfWork = new UnitOfWork(customerDbContext);

        CustomerRepository = new CustomerRepository(customerDbContext);

        //_executor = new AddCustomerServiceExecutor(CustomerRepository, mapper, unitOfWork);

    } 
    
    public static IEnumerable<AddCustomerService> Customer
    {
        get
        {
            yield return Fixture.Build<AddCustomerService>()
                .Without(l => l.FirstName)
                .Without(l => l.LastName)
                .Without(l => l.Email)
                .Without(l => l.Gender)
                .Without(l => l.Birthday)
                .Without(l => l.UpdateAt)
                .Without(l => l.IsActive)
                .Without(l => l.Status)
                .Without(l => l.ContactNumbers)
                .Create();
        }
    }

    // TODO: Terminar los test
    // por falta de tiempo no pude implementar los test
    // [Test, TestCaseSource(nameof(Customer))]
    // public async Task AddNewCustomer_Return_400_ValidationNotPassed_FieldAreRequired(AddCustomerService customer)
    // {
    //     // Arrange
    //     
    //     // Act
    //     
    //     var result = await _executor.Execute(customer);
    //     
    //
    //     // Assert
    //     
    //     Assert.That(result.Succeeded, Is.False);
    //     Assert.That(result.Data, Is.Not.Null);
    //     // Assert.That(result.Da);
    //     
    // }
}