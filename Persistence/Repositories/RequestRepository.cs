using System.Linq.Expressions;
using Application.Abstractions.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class RequestRepository : BaseRepository<Request>, IRequestRepository
{
    public RequestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Request>> GetAllRequestAsync()
    {
        return await _context.Requests
        .Include( x=> x.Product)
        .ToListAsync(); 
        
    }

    public async Task<IList<Request>> GetAllSelected(Expression<Func<Request, bool>> expression)
    {
        return await _context.Requests
        .Include( x=> x.Product)
        .Where(x => x.IsDeleted == false && x.IsApproved == true)
        .ToListAsync(); 
    }

    public async Task<Request> GetRequestAsync(string id)
    {
        return await _context.Requests
        .Include( x=> x.Product)
        .Where(x => x.IsDeleted == false && x.Id == id)
        .SingleOrDefaultAsync();
    }

    public async Task<Request> GetSelectedRequestAsync(Expression<Func<Request, bool>> expression)
    {
        return await _context.Requests
        .Include( x=> x.Product)
        .Where(x => x.IsDeleted == false )
        .SingleOrDefaultAsync(expression);
    }

    public async Task<decimal> GetSumOfApprovedRequestOfTheMonthAsync(int month, int year)
    {
        return await _context.Requests
        .Where(x => x.CreatedAt.Month == month && x.CreatedAt.Year == year && x.IsApproved == true)
        .SumAsync(x => x.Cost);
       
    }

    public async Task<decimal> GetSumOfApprovedRequestForTheYearAsync(int year)
    {
         return await _context.Requests
        .Where(x =>x.CreatedAt.Year == year && x.IsApproved == true)
        .SumAsync(x => x.Cost);
    }

    public async Task<decimal> GetSumOfApprovedReqeustForTheMonthAsync()
    {
        return await _context.Requests
            .Where(x => x.CreatedAt.Month == DateTime.Now.Month && x.IsApproved == true)
            .SumAsync(x => x.Cost);
    }

    public async Task<decimal> GetSumOfApprovedRequestForTheYearAsync()
    {
        return await _context.Requests
            .Where(x => x.CreatedAt.Year == DateTime.Now.Year && x.IsApproved == true)
            .SumAsync(x => x.Cost);
    }

}
