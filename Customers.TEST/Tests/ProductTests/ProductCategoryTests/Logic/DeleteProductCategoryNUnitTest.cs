using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Extentions;
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
public class DeleteProductCategoryNUnitTest
{
    private static readonly Fixture Fixture = new ();
    private static byte _primaryKeyId = (byte) new Random().Next(1, 255);
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductCategoryRepository _productCategoryRepository;
    private DeleteProductCategoryServiceExecutor _deleteProductCategoryServiceExecutor;
    
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
        _deleteProductCategoryServiceExecutor = new DeleteProductCategoryServiceExecutor(_productCategoryRepository, mapper, unitOfWork);
        
        // Add data to the in-memory database
        var productCategory = new Fixture().Build<ProductCategory>()
            .With(x => x.Category, "Category")
            .With(x => x.Description, "Description")
            .With(x => x.Id, _primaryKeyId)
            .Create();
        
        await _customerDbContext.ProductCategories.AddAsync(productCategory);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> DeleteProductCategoryService
    {
        get
        {
            yield return new TestCaseData(
                Fixture.Build<DeleteProductCategoryService>()
                    .Without(x => x.ProductCategoryId)
                    .With(x => x.Category, "CategoryDeleted")
                    .With(x => x.Description, "DescriptionDeleted")
                    .Create(),
                _primaryKeyId

            );

        }

    }
    
    [Test]
    [TestCaseSource(nameof(DeleteProductCategoryService))]
    public async Task DeleteProductCategory_ShouldReturn_NewObject(DeleteProductCategoryService deleteProductCategoryService, byte id)
    {
        // Arrange
        
        // Act
        var result = await _deleteProductCategoryServiceExecutor.Execute(new DeleteProductCategoryService {ProductCategoryId = id});
        var productCategoryId = (byte) result.Data;
        var productCategoryFromDb = await _customerDbContext.ProductCategories.FindAsync(id);
        
        // Assert
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<object>>());
        Assert.That(result.Message, Is.EqualTo("removed"));
        Assert.That(productCategoryId, Is.EqualTo(_primaryKeyId));
        Assert.Null(productCategoryFromDb);
    }
}