using Application.Repository.Interfaces;
using Application.Services.Product.Image;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers.v1.Product;
 
[Route("api/v1/products/images")]
public class ProductImageController: BaseApiController<ProductImageController>, IBaseApiControllerRepository<string, AddProductImageService, UpdateProductImageService>
{
    private readonly IProductImageRepository _repository;
    private readonly IValidator<AddProductImageService> _addValidator;
    private readonly IValidator<UpdateProductImageService> _updateValidator;

    public ProductImageController(IProductImageRepository repository, IValidator<AddProductImageService> addValidator, IValidator<UpdateProductImageService> updateValidator)
    {
        _repository = repository;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }
    
    public async Task<IActionResult> Get(int pageNumber, int pageSize)
    {
        var executor = new GetProductImagesServiceExecutor(_repository, _mapper, _unitOfWork);
        var result = await executor.Execute(new GetProductImagesService(pageNumber = pageNumber, pageSize = pageSize));
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Get(string id)
    {
        var executor = new GetByIdProductImageServiceExecutor(_repository, _mapper, _unitOfWork);
        var result = await executor.Execute(new GetByIdProductImageService{ ProductImageId = id});
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }
    
    [HttpGet("{productId}/images")]
    public async Task<IActionResult> GetProductImages(string productId, int pageNumber, int pageSize)
    {
        var executor = new GetProductImagesByProductIdServiceExecutor(_repository, _mapper, _unitOfWork);
        var result = await executor.Execute(new GetProductImagesByProductIdService(productId, pageNumber, pageSize));
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Post(AddProductImageService entity)
    {
        var executor = new AddProductImageServiceExecutor(_repository, _mapper, _unitOfWork, _addValidator);
        var result = await executor.Execute(entity);
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Put(string id, UpdateProductImageService entity)
    {
        var executor = new UpdateProductImageServiceExecutor(_repository, _mapper, _unitOfWork, _updateValidator);
        var result = await executor.Execute(entity);
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Delete(string id)
    {
        var executor = new DeleteProductImageServiceExecutor(_repository, _mapper, _unitOfWork);
        var result = await executor.Execute(new DeleteProductImageService() { ProductImageId = id });
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }
}