using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository.Implementations;
using Application.Services.Product.Image;
using Application.Services.Product.Image;
using AutoFixture;
using AutoMapper;
using Customers.INFRASTRUCTURE.Repository.Implementations;
using Customers.TEST.Configurations;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Domain.Entities.Products;
using Domain.Entities.Types;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.ProductTests.ProductImageTests.Logic;

[TestFixture]
public class GetProductImagesNUnitTest
{
    private static readonly Fixture Fixture = new ();
    private static byte _primaryKeyId = (byte) new Random().Next(1, 255);
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductImageRepository _productImageRepository;
    private GetProductImagesServiceExecutor _getProductImagesServiceExecutor;
    
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
        _productImageRepository = new ProductImageRepository(_customerDbContext);
        _getProductImagesServiceExecutor = new GetProductImagesServiceExecutor(_productImageRepository, mapper, unitOfWork);
        
        // Add data to the in-memory database
        var productImages = Fixture.CreateMany<ProductImage>(20)
            .ToList();
        
        await _customerDbContext.ProductImages.AddRangeAsync(productImages);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> GetProductImagesService
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
    [TestCaseSource(nameof(GetProductImagesService))]
    public async Task GetProductImages_ShouldReturn_OneObject(int pageNumber, int pageSize)
    {
        // Arrange
        
        // Act
        var result = await _getProductImagesServiceExecutor.Execute(new GetProductImagesService(pageNumber, pageSize));
        
        // Assert
        
        Assert.That(result.Data.Count, Is.EqualTo(10));
        //Assert.That(result, Is.TypeOf<ApiResponse.Response<ProductImageDto>>());
    }
}