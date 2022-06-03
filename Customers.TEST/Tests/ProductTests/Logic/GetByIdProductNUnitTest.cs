using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Product;
using Application.Extentions;
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
public class GetByIdProductNUnitTest
{
    private static readonly Fixture Fixture = new ();
    private static string _primaryKeyId = Guid.NewGuid().ToString();
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductRepository _productRepository;
    private GetByIdProductServiceExecutor _getByIdProductServiceExecutor;
    
    [SetUp]
    public async Task Setup()
    {
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => Fixture.Behaviors.Remove(b));        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: $"{nameof(CustomerDbContext)}-{Guid.NewGuid()}").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        
        var mapper = new MapperConfiguration(c => c.AddProfile(new Mappings())).CreateMapper();
        _customerDbContext = new CustomerDbContext(options);
        var unitOfWork = new UnitOfWork(_customerDbContext);
        _productRepository = new ProductRepository(_customerDbContext);
        _getByIdProductServiceExecutor = new GetByIdProductServiceExecutor(_productRepository, mapper, unitOfWork);
        
        // Add data to the in-memory database
        var product = Fixture.Build<Product>()
            .With(x => x.Id, _primaryKeyId)
            .Create();
        
        await _customerDbContext.Products.AddAsync(product);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> GetByIdProductService
    {
        get
        {
            yield return new TestCaseData(
                _primaryKeyId
            );

        }

    }
    
    [Test]
    [TestCaseSource(nameof(GetByIdProductService))]
    public async Task GetByIdProduct_ShouldReturn_OneObject(string id)
    {
        // Arrange
        
        // Act
        var result = await _getByIdProductServiceExecutor.Execute(new GetByIdProductService {ProductId = id});
        
        // Assert
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<ProductDto>>());
    }
}