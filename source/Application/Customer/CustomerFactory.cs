using CleanArchitecture.Domain;

namespace CleanArchitecture.Application;

public sealed record CustomerFactory : ICustomerFactory
{
    public Customer Create(AddCustomerRequest request) => new(default, request.Name);

    public Customer Create(UpdateCustomerRequest request) => new(request.Id, request.Name);

    public CustomerModel Create(Customer customer) => new(customer.Id, customer.Name);
}
