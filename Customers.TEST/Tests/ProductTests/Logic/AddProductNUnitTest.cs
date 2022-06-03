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
using Domain.Entities.Types;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.ProductTests.Logic;

[TestFixture]
public class AddProductNUnitTest
{
    private static readonly Fixture Fixture = new Fixture();
    private static string _primaryKeyId = Guid.NewGuid().ToString();
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductRepository _productRepository;
    private AddProductServiceExecutor _addProductServiceExecutor;
    private static ProductCategory ProductCategory = new ProductCategory()
    {
        Id = 2,
        Category = "ProductCategory Test 2",
        Description = "ProductCategory Test 2 Description"
    };

    [SetUp]
    public async Task Setup()
    {
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: $"{nameof(CustomerDbContext)}-{Guid.NewGuid()}").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        
        var mapConfig = new MapperConfiguration(c =>
        {
            c.AddProfile(new Mappings());
        });

        _customerDbContext = new CustomerDbContext(options);
        await _customerDbContext.ProductCategories.AddAsync(ProductCategory);
        await _customerDbContext.SaveChangesAsync();
        var mapper = mapConfig.CreateMapper();
        var unitOfWork = new UnitOfWork(_customerDbContext);
        _productRepository = new ProductRepository(_customerDbContext);
        var validator = new ProductValidation();
        _addProductServiceExecutor = new AddProductServiceExecutor(_productRepository, mapper, unitOfWork, validator);

    }
    
    public static IEnumerable<AddProductService> GetAddProductService
    {
        get
        {
            yield return Fixture.Build<AddProductService>()
                .With(x => x.Price, 120.00m)
                .With(x => x.Quantity, 10)
                .With(x => x.Name, "Test Product")
                .With(x => x.Description, "Test Description")
                .With(x => x.ProductCategoryId, ProductCategory.Id)
                .Without(x => x.Category)
                .With(x => x.Images, new List<ProductImageDto>
                {
                    new ProductImageDto
                    {
                        ProductImageId = _foreignKeyId,
                        Main = "Image Url 1",
                        Description = "Image Description 1"
                    }
                })
                .Create();

        }
        
    }
    
    [Test]
    [TestCaseSource(nameof(GetAddProductService))]
    public async Task AddProduct_ShouldReturn_NewObject(AddProductService addProductService)
    {
        var result = await _addProductServiceExecutor
            .Execute(addProductService);
        
        var product = (ProductDto) result.Data;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<object>>());
        Assert.That(product.Price, Is.EqualTo(120.00m));
        Assert.That(product.Quantity, Is.EqualTo(10));
        Assert.That(product.ProductCategoryId, Is.EqualTo(ProductCategory.Id));
        Assert.That(product.Category, Is.Not.Null);
        Assert.That(product.Images, Is.Not.Null);
        Assert.That(product.Images!.Count, Is.EqualTo(1));
        Assert.That(product.Images!.FirstOrDefault(), Is.Not.Null);
        Assert.That(product.Images!.FirstOrDefault()!.Main, Is.EqualTo("Image Url 1"));

    }
}