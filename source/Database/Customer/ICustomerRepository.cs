using CleanArchitecture.Domain;

namespace CleanArchitecture.Database;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);

    Task DeleteAsync(long id);

    Task<Customer> GetAsync(long id);

    Task<IList<Customer>> ListAsync();

    Task UpdateAsync(Customer customer);
}
