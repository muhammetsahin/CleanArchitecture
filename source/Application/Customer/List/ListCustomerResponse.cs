namespace CleanArchitecture.Application;

public sealed record ListCustomerResponse(IEnumerable<CustomerModel> Customers);
