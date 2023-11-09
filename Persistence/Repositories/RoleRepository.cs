using System.Linq.Expressions;
using Application.Abstractions.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Role>> GetAllRoleAsync()
        {
            return await _context.Roles
            .Include(r => r.UserRoles)
            .ToListAsync();
        }

        public async Task<Role> GetRoleAsync(string id)
        {
            return await _context.Roles
            .Include(r => r.UserRoles)
            .SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role> GetRoleAsync(Expression<Func<Role, bool>> expression)
        {
             return await _context.Roles
            .Include(r => r.UserRoles)
            .SingleOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<Role>> GetSelectedRoleAsync(Expression<Func<Role, bool>> expression)
        {
             return await _context.Roles
            .Include(r => r.UserRoles)
            .ToListAsync();
        }
}

