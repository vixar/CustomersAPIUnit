using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Implementations;
using Application.Services.Product;
using Application.Validations.Product;
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
public class UpdateProductNUnitTest
{
    private static Fixture Fixture = new Fixture();

    private static string _primaryKeyId = Guid.NewGuid().ToString();
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductRepository _productRepository;
    private UpdateProductServiceExecutor _updateProductServiceExecutor;
    
    [SetUp]
    public async Task Setup()
    {
        Fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => Fixture.Behaviors.Remove(b));  
        
        Fixture.Behaviors
            .Add(new OmitOnRecursionBehavior());
        


        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: $"{nameof(CustomerDbContext)}-{Guid.NewGuid()}").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        
        var mapper = new MapperConfiguration(c => c.AddProfile(new Mappings())).CreateMapper();
        _customerDbContext = new CustomerDbContext(options);
        var unitOfWork = new UnitOfWork(_customerDbContext);
        _productRepository = new ProductRepository(_customerDbContext);
        var validator = new ProductValidation();
        _updateProductServiceExecutor = new UpdateProductServiceExecutor(_productRepository, mapper, unitOfWork, validator);
        
        // Add data to the in-memory database

        var product =  Fixture.Build<Product>()
            .With(x => x.Id, _primaryKeyId)
            .With(x => x.Name, "Produc 1")
            .With(x => x.Price, 130.00m)
            .With(x => x.Quantity, 14)
            .With(x => x.Description, "Description Product 1")
            .Create();
        
        await _customerDbContext.Products.AddAsync(product);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> UpdateProductService
    {
        
        get
        {
            yield return new TestCaseData(
                Fixture.Build<UpdateProductService>()
                    .Without(x => x.ProductId)
                    .Without(x => x.Category)
                    .Without(x => x.Images)
                    .With(x => x.Name, "Produc 1")
                    .With(x => x.Price, 135.00m)
                    .With(x => x.Quantity, 9)
                    .Create(),
                _primaryKeyId

            );

        }

    }
    
    [Test]
    [TestCaseSource(nameof(UpdateProductService))]
    public async Task UpdateProduct_ShouldReturn_NewObject(UpdateProductService updateProductService, string id)
    {
        // arrange
        updateProductService.ProductId = id;
        var result = await _updateProductServiceExecutor.Execute(updateProductService);

        var product = (ProductDto) result.Data;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<object>>());
        Assert.That(result.Message, Is.EqualTo("updated"));
        Assert.That(product.ProductId, Is.EqualTo(_primaryKeyId));
        Assert.That(product.Price, Is.EqualTo(135.00m));
        Assert.That(product.Quantity, Is.EqualTo(9));
        Assert.That((DateTime.Now - product.UpdateAt)?.TotalMinutes, Is.LessThanOrEqualTo(5));
    }
}