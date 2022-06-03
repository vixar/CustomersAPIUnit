using Application.Repository.Interfaces;
using Application.Services.Product.Category;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers.v1.Product;
 
[Route("api/v1/products/categories")]
public class ProductCategoryController: BaseApiController<ProductCategoryController>, IBaseApiControllerRepository<byte, AddProductCategoryService, UpdateProductCategoryService>
{
    private readonly IProductCategoryRepository _repository;
    private readonly IValidator<AddProductCategoryService> _addValidator;
    private readonly IValidator<UpdateProductCategoryService> _updateValidator;

    public ProductCategoryController(IProductCategoryRepository repository, IValidator<AddProductCategoryService> addValidator, IValidator<UpdateProductCategoryService> updateValidator)
    {
        _repository = repository;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }
    
    public async Task<IActionResult> Get(int pageNumber, int pageSize)
    {
        var executor = new GetProductCategoriesServiceExecutor(_repository, _mapper, _unitOfWork);
        var result = await executor.Execute(new GetProductCategoriesService(pageNumber = pageNumber, pageSize = pageSize));
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Get(byte id)
    {
        var executor = new GetByIdProductCategoryServiceExecutor(_repository, _mapper, _unitOfWork);
        var result = await executor.Execute(new GetByIdProductCategoryService{ ProductCategoryId = id});
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Post(AddProductCategoryService entity)
    {
        var executor = new AddProductCategoryServiceExecutor(_repository, _mapper, _unitOfWork, _addValidator);
        var result = await executor.Execute(entity);
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Put(byte id, UpdateProductCategoryService entity)
    {
        var executor = new UpdateProductCategoryServiceExecutor(_repository, _mapper, _unitOfWork, _updateValidator);
        var result = await executor.Execute(entity);
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }

    public async Task<IActionResult> Delete(byte id)
    {
        var executor = new DeleteProductCategoryServiceExecutor(_repository, _mapper, _unitOfWork);
        var result = await executor.Execute(new DeleteProductCategoryService() { ProductCategoryId = id });
        return result.Succeeded
            ? Ok(result)
            : BadRequest(result.Message);
    }
}