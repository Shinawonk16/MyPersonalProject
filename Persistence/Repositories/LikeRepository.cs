using System.Linq.Expressions;
using Application.Abstractions.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class LikeRepository : BaseRepository<Like>, ILikeRepository
{
    public LikeRepository(ApplicationDbContext context):base(context)
    {
        
    }
    public async Task<IEnumerable<Like>> GetLikesAsync(string reveiwId)
    {
        return await _context.Likes
        .Include(u => u.User)
        .Where(c => c.IsDeleted == false && c.Review.Id == reveiwId)
        .ToListAsync();
    }

    public async Task<Like> GetLikeAsync(Expression<Func<Like, bool>> expression)
    {
         return await _context.Likes
        .Include(u => u.User)
        .ThenInclude(u => u.Customer)
        .Where(c => c.IsDeleted == false)
        .SingleOrDefaultAsync();
    }
}