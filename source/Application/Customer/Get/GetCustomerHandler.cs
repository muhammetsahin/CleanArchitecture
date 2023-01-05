using CleanArchitecture.CrossCutting;
using CleanArchitecture.Database;

namespace CleanArchitecture.Application;

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
