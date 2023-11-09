using Application.Abstractions.IRepository;
using Domain.Common;
using Persistence.Context;

namespace Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        await _context.SaveChangesAsync();
        return entity;
    }
}
