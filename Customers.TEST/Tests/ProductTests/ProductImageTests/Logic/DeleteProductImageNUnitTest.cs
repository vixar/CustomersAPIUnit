using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
public class DeleteProductImageNUnitTest
{
    private static readonly Fixture Fixture = new ();
    private static string _primaryKeyId = Guid.NewGuid().ToString();
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductImageRepository _productImageRepository;
    private DeleteProductImageServiceExecutor _deleteProductImageServiceExecutor;
    
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
        _productImageRepository = new ProductImageRepository(_customerDbContext);
        _deleteProductImageServiceExecutor = new DeleteProductImageServiceExecutor(_productImageRepository, mapper, unitOfWork);
        
        // Add data to the in-memory database
        var productImage = Fixture.Build<ProductImage>()
            .With(x => x.Id, _primaryKeyId)
            .Create();
        
        await _customerDbContext.ProductImages.AddAsync(productImage);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> DeleteProductImageService
    {
        get
        {
            yield return new TestCaseData(
                _primaryKeyId
            );

        }

    }
    
    [Test]
    [TestCaseSource(nameof(DeleteProductImageService))]
    public async Task DeleteProductImage_ShouldReturn_NewObject(string id)
    {
        // Arrange
        
        // Act
        var result = await _deleteProductImageServiceExecutor.Execute(new DeleteProductImageService {ProductImageId = id});
        var productImageId = (string) result.Data;
        var productImageFromDb = await _customerDbContext.ProductImages.FindAsync(id);
        
        // Assert
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<object>>());
        Assert.That(result.Message, Is.EqualTo("removed"));
        Assert.That(productImageId, Is.EqualTo(_primaryKeyId));
        Assert.Null(productImageFromDb);
    }
}