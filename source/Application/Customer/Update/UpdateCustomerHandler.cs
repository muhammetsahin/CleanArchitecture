using CleanArchitecture.CrossCutting;
using CleanArchitecture.Database;

namespace CleanArchitecture.Application;

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
