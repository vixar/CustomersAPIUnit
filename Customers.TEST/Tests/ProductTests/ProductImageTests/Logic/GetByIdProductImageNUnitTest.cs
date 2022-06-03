using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Implementations;
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
public class GetByIdProductImageNUnitTest
{
    private static readonly Fixture Fixture = new ();
    private static string _primaryKeyId = Guid.NewGuid().ToString();
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductImageRepository _productImageRepository;
    private GetByIdProductImageServiceExecutor _getByIdProductImageServiceExecutor;
    
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
        _productImageRepository = new ProductImageRepository(_customerDbContext);
        _getByIdProductImageServiceExecutor = new GetByIdProductImageServiceExecutor(_productImageRepository, mapper, unitOfWork);
        
        // Add data to the in-memory database
        var productImage = Fixture.Build<ProductImage>()
            .With(x => x.Id, _primaryKeyId)
            .Create();
        
        await _customerDbContext.ProductImages.AddAsync(productImage);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> GetByIdProductImageService
    {
        get
        {
            yield return new TestCaseData(
                _primaryKeyId
            );

        }

    }
    
    [Test]
    [TestCaseSource(nameof(GetByIdProductImageService))]
    public async Task GetByIdProductImage_ShouldReturn_OneObject(string id)
    {
        // Arrange
        
        // Act
        var result = await _getByIdProductImageServiceExecutor.Execute(new GetByIdProductImageService {ProductImageId = id});
        
        // Assert
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<ProductImageDto>>());
    }
}