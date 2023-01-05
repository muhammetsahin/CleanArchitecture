using CleanArchitecture.Domain;

namespace CleanArchitecture.Application;

public interface ICustomerFactory
{
    Customer Create(AddCustomerRequest request);

    Customer Create(UpdateCustomerRequest request);

    CustomerModel Create(Customer customer);
}
