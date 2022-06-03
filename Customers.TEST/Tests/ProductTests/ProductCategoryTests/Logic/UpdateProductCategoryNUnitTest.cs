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
using Domain.Entities.Types;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.ProductTests.ProductCategoryTests.Logic;

[TestFixture]
public class UpdateProductCategoryNUnitTest
{
    private static readonly Fixture Fixture = new ();
    private static byte _primaryKeyId = (byte) new Random().Next(1, 255);
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductCategoryRepository _productCategoryRepository;
    private UpdateProductCategoryServiceExecutor _updateProductCategoryServiceExecutor;
    
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
        var validator = new ProductCategoryValidation();
        _updateProductCategoryServiceExecutor = new UpdateProductCategoryServiceExecutor(_productCategoryRepository, mapper, unitOfWork, validator);
        
        // Add data to the in-memory database
        var productCategory = new Fixture().Build<ProductCategory>()
            .With(x => x.Category, "Category")
            .With(x => x.Description, "Description")
            .With(x => x.Id, _primaryKeyId)
            .Create();
        
        await _customerDbContext.ProductCategories.AddAsync(productCategory);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> UpdateProductCategoryService
    {
        get
        {
            yield return new TestCaseData(
                Fixture.Build<UpdateProductCategoryService>()
                    .Without(x => x.ProductCategoryId)
                    .With(x => x.Category, "CategoryUpdated")
                    .With(x => x.Description, "DescriptionUpdated")
                    .Without(x => x.UpdateAt)
                    .Create(),
                _primaryKeyId

            );

        }

    }
    
    [Test]
    [TestCaseSource(nameof(UpdateProductCategoryService))]
    public async Task UpdateProductCategory_ShouldReturn_NewObject(UpdateProductCategoryService updateProductCategoryService, object id)
    {
        //assert
        var result = await _updateProductCategoryServiceExecutor.Execute(new UpdateProductCategoryService(updateProductCategoryService, (byte)id));

        var productCategory = (ProductCategoryDto) result.Data;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<object>>());
        Assert.That(productCategory.ProductCategoryId, Is.EqualTo(_primaryKeyId));
        Assert.That(productCategory.Category, Is.EqualTo("CategoryUpdated"));
        Assert.That(productCategory.Description, Is.EqualTo("DescriptionUpdated"));
        Assert.That(productCategory.UpdateAt?.Hour, Is.EqualTo(DateTime.Now.Hour));
    }
}