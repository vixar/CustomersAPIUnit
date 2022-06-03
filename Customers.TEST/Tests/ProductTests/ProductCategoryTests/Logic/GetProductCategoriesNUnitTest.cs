using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository.Implementations;
using Application.Services.Product.Category;
using AutoFixture;
using AutoMapper;
using Customers.INFRASTRUCTURE.Repository.Implementations;
using Customers.TEST.Configurations;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities.Types;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.ProductTests.ProductCategoryTests.Logic;

[TestFixture]
public class GetProductCategoriesNUnitTest
{
    private static readonly Fixture Fixture = new ();
    private static byte _primaryKeyId = (byte) new Random().Next(1, 255);
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductCategoryRepository _productCategoryRepository;
    private GetProductCategoriesServiceExecutor _getProductCategoriesServiceExecutor;
    
    [SetUp]
    public async Task Setup()
    {
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: $"{nameof(CustomerDbContext)}-{Guid.NewGuid()}").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        
        var mapper = new MapperConfiguration(c => c.AddProfile(new Mappings())).CreateMapper();
        _customerDbContext = new CustomerDbContext(options);
        var unitOfWork = new UnitOfWork(_customerDbContext);
        _productCategoryRepository = new ProductCategoryRepository(_customerDbContext);
        _getProductCategoriesServiceExecutor = new GetProductCategoriesServiceExecutor(_productCategoryRepository, mapper, unitOfWork);
        
        // Add data to the in-memory database
        var productCategories = new Fixture().CreateMany<ProductCategory>(20)
            .ToList();
        
        await _customerDbContext.ProductCategories.AddRangeAsync(productCategories);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> GetProductCategoriesService
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
    [TestCaseSource(nameof(GetProductCategoriesService))]
    public async Task GetProductCategories_ShouldReturn_OneObject(int pageNumber, int pageSize)
    {
        // Arrange
        
        // Act
        var result = await _getProductCategoriesServiceExecutor.Execute(new GetProductCategoriesService(pageNumber, pageSize));
        
        // Assert
        
        Assert.That(result.Data.Count, Is.EqualTo(10));
        //Assert.That(result, Is.TypeOf<ApiResponse.Response<ProductCategoryDto>>());
    }
}