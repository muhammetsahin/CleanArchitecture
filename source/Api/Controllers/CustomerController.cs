using CleanArchitecture.Application;
using CleanArchitecture.CrossCutting;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api;

[ApiController]
[Route("customers")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public Task<IActionResult> AddAsync(AddCustomerRequest request)
    {
        return _mediator.HandleAsync<AddCustomerRequest, AddCustomerResponse>(request).PostResultAsync();
    }

    [HttpDelete("{id:long}")]
    public Task<IActionResult> DeleteAsync(long id)
    {
        return _mediator.HandleAsync(new DeleteCustomerRequest(id)).DeleteResultAsync();
    }

    [HttpGet("{id:long}")]
    public Task<IActionResult> GetAsync(long id)
    {
        return _mediator.HandleAsync<GetCustomerRequest, GetCustomerResponse>(new GetCustomerRequest(id)).GetResultAsync();
    }

    [HttpGet]
    public Task<IActionResult> ListAsync()
    {
        return _mediator.HandleAsync<ListCustomerRequest, ListCustomerResponse>(new ListCustomerRequest()).GetResultAsync();
    }

    [HttpPut("{id:long}")]
    public Task<IActionResult> UpdateAsync(UpdateCustomerRequest request)
    {
        return _mediator.HandleAsync(request).PutResultAsync();
    }
}
