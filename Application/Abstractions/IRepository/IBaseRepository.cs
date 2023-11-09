namespace Application.Abstractions.IRepository;

public interface IBaseRepository<T>
{
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<int> SaveAsync();
}