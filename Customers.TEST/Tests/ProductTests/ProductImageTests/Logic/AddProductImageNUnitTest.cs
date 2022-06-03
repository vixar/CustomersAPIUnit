using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Implementations;
using Application.Services.Product.Image;
using Application.Validations.Product;
using AutoFixture;
using AutoMapper;
using Customers.INFRASTRUCTURE.Repository.Implementations;
using Customers.TEST.Configurations;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.ProductTests.ProductImageTests.Logic;

[TestFixture]
public class AddProductImageNUnitTest
{
    private static readonly Fixture Fixture = new Fixture();
    private static string _primaryKeyId = Guid.NewGuid().ToString();
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductImageRepository _productImageRepository;
    private AddProductImageServiceExecutor _addProductImageServiceExecutor;
    
    public async Task Setup()
    {
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => Fixture.Behaviors.Remove(b));        
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: $"{nameof(CustomerDbContext)}-{Guid.NewGuid()}").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        
        var mapConfig = new MapperConfiguration(c =>
        {
            c.AddProfile(new Mappings());
        });

        _customerDbContext = new CustomerDbContext(options);
        var mapper = mapConfig.CreateMapper();
        var unitOfWork = new UnitOfWork(_customerDbContext);
        _productImageRepository = new ProductImageRepository(_customerDbContext);
        var validator = new ProductImageValidation();
        _addProductImageServiceExecutor = new AddProductImageServiceExecutor(_productImageRepository, mapper, unitOfWork, validator);
        
        
    }
    
    public static IEnumerable<AddProductImageService> GetAddProductImageService
    {
        get
        {       
            yield return Fixture.Build<AddProductImageService>()
                .With(x => x.ProductImageId, _primaryKeyId)
                .Create();
        }
        
    }
    
    [Test]
    [TestCaseSource(nameof(GetAddProductImageService))]
    public async Task AddProductImage_ShouldReturn_NewObject(AddProductImageService addProductImageService)
    {
        await Setup();
        
        var result = await _addProductImageServiceExecutor
            .Execute(addProductImageService);
        
        var productImage = (ProductImageDto) result.Data;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<object>>());
        Assert.That(productImage.ProductImageId, Is.EqualTo(_primaryKeyId));
    }
}