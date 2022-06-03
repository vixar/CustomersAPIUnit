using Application.Services.Customer;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers;

public interface IBaseApiControllerRepository<in T, in T1, in T2>
{
    [HttpGet]
    Task<IActionResult> Get(int pageNumber, int pageSize);

    [HttpGet("{id}")]
    Task<IActionResult> Get(T id);

    [HttpPost]
    Task<IActionResult> Post(T1 entity);

    [HttpPut("{id}")]
    Task<IActionResult> Put(T id, T2 entity);

    [HttpDelete("{id}")]
    Task<IActionResult> Delete(T id);

}