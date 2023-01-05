using CleanArchitecture.CrossCutting;
using CleanArchitecture.Database;

namespace CleanArchitecture.Application;

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
