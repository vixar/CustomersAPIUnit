using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository.Implementations;
using Application.Services.Product;
using AutoFixture;
using AutoMapper;
using Customers.INFRASTRUCTURE.Repository.Implementations;
using Customers.TEST.Configurations;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.ProductTests.Logic;

[TestFixture]
public class GetProductsNUnitTest
{
    private static readonly Fixture Fixture = new ();
    private static byte _primaryKeyId = (byte) new Random().Next(1, 255);
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductRepository _productRepository;
    private GetProductsServiceExecutor _getProductsServiceExecutor;
    
    [SetUp]
    public async Task Setup()
    {
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => Fixture.Behaviors.Remove(b));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: $"{nameof(CustomerDbContext)}-{Guid.NewGuid()}").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        
        var mapper = new MapperConfiguration(c => c.AddProfile(new Mappings())).CreateMapper();
        _customerDbContext = new CustomerDbContext(options);
        var unitOfWork = new UnitOfWork(_customerDbContext);
        _productRepository = new ProductRepository(_customerDbContext);
        _getProductsServiceExecutor = new GetProductsServiceExecutor(_productRepository, mapper, unitOfWork);
        
        // Add data to the in-memory database
        var products = Fixture.CreateMany<Product>(20)
            .ToList();
        
        await _customerDbContext.Products.AddRangeAsync(products);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> GetProductsService
    {
        get
        {
            yield return new TestCaseData(
                2,
                10
            );

        }

    }
    
    [Test]
    [TestCaseSource(nameof(GetProductsService))]
    public async Task GetProducts_ShouldReturn_OneObject(int pageNumber, int pageSize)
    {
        // Arrange
        
        // Act
        var result = await _getProductsServiceExecutor.Execute(new GetProductsService(pageNumber, pageSize));
        
        // Assert
        
        Assert.That(result.Data.Count, Is.EqualTo(10));
        //Assert.That(result, Is.TypeOf<ApiResponse.Response<ProductDto>>());
    }
}