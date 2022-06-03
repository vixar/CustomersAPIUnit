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
using Domain.Entities.Products;
using Domain.Entities.Types;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.ProductTests.ProductImageTests.Logic;

[TestFixture]
public class PatchProductImageNUnitTest
{
    private static readonly Fixture Fixture = new ();
    private static readonly string _primaryKeyId = Guid.NewGuid().ToString();
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductImageRepository _productImageRepository;
    private PatchProductImageServiceExecutor _patchProductImageServiceExecutor;
    
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
        var validator = new ProductImageValidation();
        _patchProductImageServiceExecutor = new PatchProductImageServiceExecutor(_productImageRepository, mapper, unitOfWork, validator);
        
        // Add data to the in-memory database
        var productImage = Fixture.Build<ProductImage>()
            .With(x => x.Main, "Image")
            .With(x => x.Description, "Description")
            .With(x => x.Id, _primaryKeyId)
            .Without(x => x.UpdateAt)
            .Create();
        
        await _customerDbContext.ProductImages.AddAsync(productImage);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> PatchProductImageService
    {
        get
        {
            yield return new TestCaseData(
                new JsonPatchDocument<ProductImageDto>().Replace(x => x.Main, "ImagePatched"),
                _primaryKeyId
            );

        }

    }
    
    [Test]
    [TestCaseSource(nameof(PatchProductImageService))]
    public async Task PatchProductImage_ShouldReturn_NewObject(JsonPatchDocument<ProductImageDto> patchEntity, string id)
    {
        //assert
        var result = await _patchProductImageServiceExecutor.Execute(new PatchProductImageService(patchEntity, id));

        var productImage = (ProductImageDto) result.Data;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<object>>());
        Assert.That(productImage.ProductImageId, Is.EqualTo(_primaryKeyId));
        Assert.That(productImage.Main, Is.EqualTo("ImagePatched"));
        Assert.That(productImage.UpdateAt?.Hour, Is.EqualTo(DateTime.Now.Hour));
    }
}