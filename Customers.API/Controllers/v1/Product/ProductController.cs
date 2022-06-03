using Application.Repository.Interfaces;
using Application.Services.Product;
using Application.Services.Product.Image;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers.v1.Product;
 
[Route("api/v1/products")]
public class ProductController: BaseApiController<ProductController>, IBaseApiControllerRepository<string, AddProductService, UpdateProductService>
{
    private readonly IProductRepository _repository;
    private readonly IValidator<AddProductService> _addValidator;
    private readonly IValidator<UpdateProductService> _updateValidator;

    public ProductController(IProductRepository repository, IValidator<AddProductService> addValidator, IValidator<UpdateProductService> updateValidator)
    {
        _repository = repository;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }
    
    public async Task<IActionResult> Get(int pageNumber, int pageSize)
    {
        var executor = new GetProductsServiceExecutor(_repository, _mapper, _unitOfWork);
        var result = await executor.Execute(new GetProductsService(pageNumber = pageNumber, pageSize = pageSize));
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Get(string id)
    {
        var executor = new GetByIdProductServiceExecutor(_repository, _mapper, _unitOfWork);
        var result = await executor.Execute(new GetByIdProductService{ ProductId = id});
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Post(AddProductService entity)
    {
        var executor = new AddProductServiceExecutor(_repository, _mapper, _unitOfWork, _addValidator);
        var result = await executor.Execute(entity);
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Put(string id, UpdateProductService entity)
    {
        var executor = new UpdateProductServiceExecutor(_repository, _mapper, _unitOfWork, _updateValidator);
        var result = await executor.Execute(entity);
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Delete(string id)
    {
        var executor = new DeleteProductServiceExecutor(_repository, _mapper, _unitOfWork);
        var result = await executor.Execute(new DeleteProductService() { ProductId = id });
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }
}