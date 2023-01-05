namespace CleanArchitecture.Database;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
