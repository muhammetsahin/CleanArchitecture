# Clean Architecture

This architecture is suitable for large and complex projects.

**Benefits:**

* Simple and evolutionary architecture.
* Standardized and centralized flow for validation, security, log, result, etc.
* Avoid cyclical reference between classes.
* Avoid unnecessary dependency injection in the constructor.
* Segregation by feature instead of technical type.
* Single responsibility for each request and response.
* Increase simplicity of unit testing.

## Principles and Patterns

* Clean Code
* SOLID Principles
* KISS Principle
* Common Closure Principle
* Common Reuse Principle
* Mediator Pattern
* Fail Fast Pattern
* Result Pattern
* Folder By Feature

## API

### Program

```cs
var builder = WebApplication.CreateBuilder(args);

builder.Host.Serilog();

builder.Services.AddContextInMemoryDatabase();
builder.Services.AddJsonStringLocalizer();
builder.Services.AddClassesMatchingInterfaces();
builder.Services.AddMediator();
builder.Services.AddResponseCompression();
builder.Services.AddControllers().AddJsonOptions();
builder.Services.AddSwaggerGen();

var application = builder.Build();

application.UseException();
application.UseLocalization();
application.UseSwagger();
application.UseSwaggerUI();
application.UseHttpsRedirection();
application.UseResponseCompression();
application.MapControllers();

application.Run();
```

### Controller

```cs
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
```

## APPLICATION

### Add

```cs
public sealed record AddCustomerRequest(string Name);
```

```cs
public sealed class AddCustomerRequestValidator : AbstractValidator<AddCustomerRequest>
{
    public AddCustomerRequestValidator()
    {
        RuleFor(request => request.Name).Name();
    }
}
```

```cs
public sealed record AddCustomerResponse(long Id);
```

```cs
public sealed record AddCustomerHandler : IHandler<AddCustomerRequest, AddCustomerResponse>
{
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCustomerHandler
    (
        ICustomerFactory customerFactory,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork
    )
    {
        _customerFactory = customerFactory;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AddCustomerResponse>> HandleAsync(AddCustomerRequest request)
    {
        var customer = _customerFactory.Create(request);

        await _customerRepository.AddAsync(customer);

        await _unitOfWork.SaveChangesAsync();

        var response = new AddCustomerResponse(customer.Id);

        return Result<AddCustomerResponse>.Success(response);
    }
}
```

### Delete

```cs
public sealed record DeleteCustomerRequest(long Id);
```

```cs
public sealed class DeleteCustomerRequestValidator : AbstractValidator<DeleteCustomerRequest>
{
    public DeleteCustomerRequestValidator()
    {
        RuleFor(request => request.Id).Id();
    }
}
```

```cs
public sealed record DeleteCustomerHandler : IHandler<DeleteCustomerRequest>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerHandler
    (
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork
    )
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteCustomerRequest request)
    {
        await _customerRepository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
```

### Get

```cs
public sealed record GetCustomerRequest(long Id);
```

```cs
public sealed class GetCustomerRequestValidator : AbstractValidator<GetCustomerRequest>
{
    public GetCustomerRequestValidator()
    {
        RuleFor(request => request.Id).Id();
    }
}
```

```cs
public sealed record GetCustomerResponse(CustomerModel Customer);
```

```cs
public sealed record GetCustomerHandler : IHandler<GetCustomerRequest, GetCustomerResponse>
{
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerHandler
    (
        ICustomerFactory customerFactory,
        ICustomerRepository customerRepository
    )
    {
        _customerFactory = customerFactory;
        _customerRepository = customerRepository;
    }

    public async Task<Result<GetCustomerResponse>> HandleAsync(GetCustomerRequest request)
    {
        var customer = await _customerRepository.GetAsync(request.Id);

        if (customer is null) return Result<GetCustomerResponse>.Success();

        var model = _customerFactory.Create(customer);

        var response = new GetCustomerResponse(model);

        return Result<GetCustomerResponse>.Success(response);
    }
}
```

### List

```cs
public sealed record ListCustomerRequest;
```

```cs
public sealed record ListCustomerResponse(IEnumerable<CustomerModel> Customers);
```

```cs
public sealed record ListCustomerHandler : IHandler<ListCustomerRequest, ListCustomerResponse>
{
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerRepository _customerRepository;

    public ListCustomerHandler
    (
        ICustomerFactory customerFactory,
        ICustomerRepository customerRepository
    )
    {
        _customerFactory = customerFactory;
        _customerRepository = customerRepository;
    }

    public async Task<Result<ListCustomerResponse>> HandleAsync(ListCustomerRequest request)
    {
        var customers = await _customerRepository.ListAsync();

        if (!customers.Any()) return Result<ListCustomerResponse>.Success();

        var models = customers.Select(_customerFactory.Create);

        var response = new ListCustomerResponse(models);

        return Result<ListCustomerResponse>.Success(response);
    }
}
```

### Update

```cs
public sealed record UpdateCustomerRequest(long Id, string Name);
```

```cs
public sealed class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(request => request.Id).Id();
        RuleFor(request => request.Name).Name();
    }
}
```

```cs
public sealed record UpdateCustomerHandler : IHandler<UpdateCustomerRequest>
{
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerHandler
    (
        ICustomerFactory customerFactory,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork
    )
    {
        _customerFactory = customerFactory;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateCustomerRequest request)
    {
        var customer = _customerFactory.Create(request);

        await _customerRepository.UpdateAsync(customer);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
```

### Model

```cs
public sealed record CustomerModel(long Id, string Name);
```

### Factory

```cs
public interface ICustomerFactory
{
    Customer Create(AddCustomerRequest request);

    Customer Create(UpdateCustomerRequest request);

    CustomerModel Create(Customer customer);
}
```

```cs
public sealed record CustomerFactory : ICustomerFactory
{
    public Customer Create(AddCustomerRequest request) => new(default, request.Name);

    public Customer Create(UpdateCustomerRequest request) => new(request.Id, request.Name);

    public CustomerModel Create(Customer customer) => new(customer.Id, customer.Name);
}
```
