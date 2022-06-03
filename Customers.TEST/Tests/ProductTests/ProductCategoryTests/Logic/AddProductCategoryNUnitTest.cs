using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Product;
using Application.Extentions;
using Application.Repository.Implementations;
using Application.Services.Product.Category;
using Application.Validations.Product;
using AutoFixture;
using AutoMapper;
using Customers.INFRASTRUCTURE.Repository.Implementations;
using Customers.TEST.Configurations;
using Cutomers.INFRASTRUCTURE.DbConfiguration;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.ProductTests.ProductCategoryTests.Logic;

[TestFixture]
public class AddProductCategoryNUnitTest
{
    private static readonly Fixture Fixture = new Fixture();
    private static byte _primaryKeyId = (byte) new Random().Next(1, 255);
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductCategoryRepository _productCategoryRepository;
    private AddProductCategoryServiceExecutor _addProductCategoryServiceExecutor;
    
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
        var mapper = mapConfig.CreateMapper();
        var unitOfWork = new UnitOfWork(_customerDbContext);
        _productCategoryRepository = new ProductCategoryRepository(_customerDbContext);
        var validator = new ProductCategoryValidation();
        _addProductCategoryServiceExecutor = new AddProductCategoryServiceExecutor(_productCategoryRepository, mapper, unitOfWork, validator);
        
        

    }
    
    public static IEnumerable<AddProductCategoryService> GetAddProductCategoryService
    {
        get
        {
            yield return Fixture.Build<AddProductCategoryService>()
                .With(x => x.ProductCategoryId, _primaryKeyId)
                .Create();

        }
        
    }
    
    [Test]
    [TestCaseSource(nameof(GetAddProductCategoryService))]
    public async Task AddProductCategory_ShouldReturn_NewObject(AddProductCategoryService addProductCategoryService)
    {
        await Setup();
        
        var result = await _addProductCategoryServiceExecutor
            .Execute(addProductCategoryService);
        
        var productCategory = (ProductCategoryDto) result.Data;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<object>>());
        Assert.That(productCategory.ProductCategoryId, Is.EqualTo(_primaryKeyId));
    }
}