using System.Linq.Expressions;
using Application.Abstractions.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class VerificationRepository : BaseRepository<Verification>, IVerificationRepository
{
    public VerificationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Verification> GetAsync(string id)
    {
        return await _context.Verifications
        .Include(x => x.Customer)
        .ThenInclude(x => x.User)
        .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }

    public async Task<Verification> GetAsync(Expression<Func<Verification, bool>> expression)
    {
        return await _context.Verifications
        .Include(x => x.Customer)
        .ThenInclude(x => x.User)
        .SingleOrDefaultAsync(expression);
    }
}