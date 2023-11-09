using System.Linq.Expressions;
using Application.Abstractions.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IList<Review>> GetAllReviewAsync()
    {
        return await _context.Reviews
         .Include(c => c.Customer)
         .ThenInclude(x => x.User)
         .Where(x => x.IsDeleted == false )
         .OrderByDescending(x => x.CreatedAt)
         .ToListAsync();
    }

    public async Task<IList<Review>> GetAllReviewByCustomerIdAsync(string customerId)
    {
        return await _context.Reviews
         .Include(c => c.Customer)
         .ThenInclude(x => x.User)
         .Where(x => x.IsDeleted == false && x.Customer.Id == customerId)
         .OrderByDescending(x => x.CreatedAt)
         .ToListAsync();
    }

    public async Task<IList<Review>> GetAllSelectedReviewAsync(Expression<Func<Review, bool>> expression)
    {
        return await _context.Reviews
         .Include(c => c.Customer)
         .ThenInclude(x => x.User)
         .Where(x => x.IsDeleted == false)
         .OrderByDescending(x => x.CreatedAt)
         .ToListAsync();
    }

    public async Task<Review> GetReviewAsync(Expression<Func<Review, bool>> expression)
    {
        return await _context.Reviews
         .Include(c => c.Customer)
         .ThenInclude(x => x.User)
         .Where(x => x.IsDeleted == false)
         .OrderByDescending(x => x.CreatedAt)
         .SingleOrDefaultAsync(expression);

    }

    public async Task<IList<Review>> GetReviewByProductIdAsync(string productId)
    {
         return await _context.Reviews
         .Include(c => c.Customer)
         .ThenInclude(x => x.User)
         .Where(x => x.IsDeleted == false && x.Product.Id == productId)
         .OrderByDescending(x => x.CreatedAt)
         .ToListAsync();
    }
}
