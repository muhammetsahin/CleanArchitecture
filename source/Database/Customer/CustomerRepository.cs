using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Database;

public sealed record CustomerRepository : ICustomerRepository
{
    private readonly Context _context;

    public CustomerRepository(Context context)
    {
        _context = context;
    }

    public Task AddAsync(Customer customer)
    {
        return _context.AddAsync(customer).AsTask();
    }

    public async Task DeleteAsync(long id)
    {
        var customer = await GetAsync(id);

        if (customer is null) return;

        _context.Remove(customer);
    }

    public Task<Customer> GetAsync(long id)
    {
        return _context.Set<Customer>().FindAsync(id).AsTask();
    }

    public async Task<IList<Customer>> ListAsync()
    {
        return await _context.Set<Customer>().ToListAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        var customerDatabase = await GetAsync(customer.Id);

        if (customerDatabase is null) return;

        _context.Entry(customerDatabase).State = EntityState.Detached;

        _context.Update(customer);
    }
}
