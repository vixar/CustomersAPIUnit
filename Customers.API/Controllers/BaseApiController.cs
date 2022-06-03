using Application.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers;

[ApiController]
public abstract class BaseApiController<T> : ControllerBase
{
    private IMapper? _mapperInstante;
    private IUnitOfWork? _unitOfWorkInstante;
    protected IMapper? _mapper => _mapperInstante ??= HttpContext.RequestServices.GetService<IMapper>();
    protected IUnitOfWork? _unitOfWork => _unitOfWorkInstante ??= HttpContext.RequestServices.GetService<IUnitOfWork>();
    
    

}