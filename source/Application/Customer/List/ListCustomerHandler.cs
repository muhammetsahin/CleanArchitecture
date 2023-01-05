using CleanArchitecture.CrossCutting;
using CleanArchitecture.Database;

namespace CleanArchitecture.Application;

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
