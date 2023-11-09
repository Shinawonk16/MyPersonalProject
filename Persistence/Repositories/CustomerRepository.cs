using System.Linq.Expressions;
using Application.Abstractions.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {

    }
    public async Task<bool> CheckIfEmailExistAsync(string email)
    {
        return await _context.Customers
        .Include(c => c.User)
        .Where(x => x.User.Email == email).AnyAsync();

    }

    public async Task<IEnumerable<Customer>> GetAllCustomerAsync()
    {
        return await _context.Customers
            .Include(c => c.User)
            .ThenInclude(c => c.UserRoles)
            .ThenInclude(x => x.Role)
            .Where(c => c.IsDeleted == false)
            .ToListAsync();
    }

    public async Task<Customer> GetAsync(string id)
    {
        return await _context.Customers
        .Include(c => c.User)
        .Where(c => c.IsDeleted == false)
        .SingleOrDefaultAsync(c => c.UserId == id);

    }

    public async Task<Customer> GetAsync(Expression<Func<Customer, bool>> expression)
    {
        return await _context.Customers
        .Include(c => c.User)
        .Where(c => c.IsDeleted == false)
        .SingleOrDefaultAsync(expression);
    }

    public async Task<IEnumerable<Customer>> GetSelectedAsync(Expression<Func<Customer, bool>> expression)
    {
        return await _context.Customers
        .Include(c => c.User)
        .Where(c => c.IsDeleted == false)
        .ToListAsync();
    }
}

