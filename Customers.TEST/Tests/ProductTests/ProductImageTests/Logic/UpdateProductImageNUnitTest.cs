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
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Customers.TEST.Tests.ProductTests.ProductImageTests.Logic;

[TestFixture]
public class UpdateProductImageNUnitTest
{
    private static Fixture Fixture = new Fixture();

    private static string _primaryKeyId = Guid.NewGuid().ToString();
    private static string _primaryKeyVisitId = Guid.NewGuid().ToString();
    private static string _foreignKeyId = Guid.NewGuid().ToString();
    private CustomerDbContext _customerDbContext;
    private ProductImageRepository _productImageRepository;
    private UpdateProductImageServiceExecutor _updateProductImageServiceExecutor;
    
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
        _productImageRepository = new ProductImageRepository(_customerDbContext);
        var validator = new ProductImageValidation();
        _updateProductImageServiceExecutor = new UpdateProductImageServiceExecutor(_productImageRepository, mapper, unitOfWork, validator);
        
        // Add data to the in-memory database

        var productImage =  Fixture.Build<ProductImage>()
            .With(x => x.Main, "Image")
            .With(x => x.Description, "Description")
            .With(x => x.Id, _primaryKeyId)
            .Create();
        
        await _customerDbContext.ProductImages.AddAsync(productImage);
        await _customerDbContext.SaveChangesAsync();

    }
    
    public static IEnumerable<TestCaseData> UpdateProductImageService
    {
        
        get
        {
            yield return new TestCaseData(
                Fixture.Build<UpdateProductImageService>()
                    .With(x => x.Main, "ImageUpdated")
                    .With(x => x.Description, "DescriptionUpdated")
                    .Create(),
                _primaryKeyId

            );

        }

    }
    
    [Test]
    [TestCaseSource(nameof(UpdateProductImageService))]
    public async Task UpdateProductImage_ShouldReturn_NewObject(UpdateProductImageService updateProductImageService, string id)
    {
        // arrange
        updateProductImageService.ProductImageId = id;
        var result = await _updateProductImageServiceExecutor.Execute(updateProductImageService);

        var productImage = (ProductImageDto) result.Data;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ApiResponse.Response<object>>());
        Assert.That(productImage.ProductImageId, Is.EqualTo(_primaryKeyId));
        Assert.That(productImage.Main, Is.EqualTo("ImageUpdated"));
        Assert.That(productImage.Description, Is.EqualTo("DescriptionUpdated"));
        Assert.That(productImage.UpdateAt?.Hour, Is.EqualTo(DateTime.Now.Hour));
    }
}