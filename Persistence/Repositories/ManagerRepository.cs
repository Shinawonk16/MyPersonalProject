using System.Linq.Expressions;
using Application.Abstractions.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class ManagerRepository : BaseRepository<Manager>, IManagerRepository
{
    public ManagerRepository(ApplicationDbContext context) : base(context)
    {

    }
    public async Task<bool> CheckIfEmailExistAsync(string email)
    {
        return await _context.Managers
        .Include(u => u.User)
        .Where(e => e.User.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).AnyAsync();
    }

    public async Task<IEnumerable<UserRole>> GetAllManagerAsync()
    {
        return await _context.UserRoles
        .Include(u => u.User)
        .Include(x => x.Role)
        .Where(x => x.User.IsDeleted == false && x.Role.RoleName != "customer")
        .ToListAsync();
    }

    public async Task<Manager> GetAsync(Expression<Func<Manager, bool>> expression)
    {
        return await _context.Managers
        .Include(u => u.User)
        .Where(c => c.IsDeleted == false)
        .SingleOrDefaultAsync(expression);
    }

    public async Task<UserRole> GetManagerAsync(string id)
    {
        return await _context.UserRoles
        .Include(u => u.User).Include(x => x.Role)
        .Where(x => x.User.IsDeleted == false && x.Role.RoleName != "customer")
        .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<UserRole> GetManagerByRoleAsync(string roleId)
    {
        return await _context.UserRoles
        .Include(r => r.Role)
        .Include(r => r.User)
        .Include(r => r.Role.Id == roleId)
        .Where(x => x.IsDeleted == false)
        .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Manager>> GetSelectedAsync(Expression<Func<Manager, bool>> expression)
    {
        return await _context.Managers
        .Include(u => u.User)
        .Where(c => c.IsDeleted == false)
        .ToListAsync();
    }
}

