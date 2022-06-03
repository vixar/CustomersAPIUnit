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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.ProductTests.Logic;

[TestFixture]
public class PatchProductNUnitTest
{
    private static readonly Fixture Fixture = new ();
    private static readonly string _primaryKeyId = Guid.NewGuid().ToString();
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductRepository _productRepository;
    private PatchProductServiceExecutor _patchProductServiceExecutor;
    
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
        _productRepository = new ProductRepository(_customerDbContext);
        var validator = new ProductValidation();
        _patchProductServiceExecutor = new PatchProductServiceExecutor(_productRepository, mapper, unitOfWork, validator);
        
        // Add data to the in-memory database
        var product = Fixture.Build<Product>()
            .With(x => x.Name, "Product Name")
            .With(x => x.Description, "Description")
            .With(x => x.Id, _primaryKeyId)
            .Without(x => x.UpdateAt)
            // .Without(x => x.Category)
            // .Without(x => x.Images)
            .Create();
        
        await _customerDbContext.Products.AddAsync(product);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> PatchProductService
    {
        get
        {
            yield return new TestCaseData(
                new JsonPatchDocument<ProductDto>().Replace(x => x.Name, "Product Name Patched"),
                _primaryKeyId
            );

        }

    }
    
    [Test]
    [TestCaseSource(nameof(PatchProductService))]
    public async Task PatchProduct_ShouldReturn_NewObject(JsonPatchDocument<ProductDto> patchEntity, string id)
    {
        //assert
        var result = await _patchProductServiceExecutor.Execute(new PatchProductService(patchEntity, id));

        var product = (ProductDto) result.Data;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<object>>());
        Assert.That(product.ProductId, Is.EqualTo(_primaryKeyId));
        Assert.That(product.Name, Is.EqualTo("Product Name Patched"));
        Assert.That(product.UpdateAt?.Hour, Is.EqualTo(DateTime.Now.Hour));
    }
}