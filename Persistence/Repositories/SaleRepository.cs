using System.Linq.Expressions;
using Application.Abstractions.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class SaleRepository : BaseRepository<Sales>, ISaleRepository
{
    public SaleRepository(ApplicationDbContext context) : base(context)
    {
    }


    public async Task<IList<Sales>> GetAllSaleAsync()
    {
        return await _context.Sales
        .Include(x => x.Order)
        .ThenInclude(x => x.Customer)
        .ToListAsync();
    }

    public async Task<IList<Sales>> GetAllSaleAsync(Expression<Func<Sales, bool>> expression)
    {
        return await _context.Sales
        .Include(x => x.Order)
        .ThenInclude(x => x.Customer)
        .ToListAsync();
    }

    public async Task<IList<Sales>> GetAllSalesOfTheMonthAsync()
    {
        return await _context.Sales
     .Include(x => x.Order)
     .ThenInclude(x => x.Customer)
     .Where(x => x.CreatedAt.Year == DateTime.Now.Year && x.CreatedAt.Month == DateTime.Now.Month)
     .ToListAsync();
    }

    public async Task<IList<Sales>> GetAllSalesOfTheYearAsync()
    {
        return await _context.Sales
       .Include(x => x.Order)
       .ThenInclude(x => x.Customer)
       .Where(x => x.CreatedAt.Year == DateTime.Now.Year)
       .ToListAsync();
    }

    public async Task<Sales> GetSaleAsync(string id)
    {
        return await _context.Sales
        .Include(x => x.Order)
        .ThenInclude(x => x.Customer)
        .Where(x => x.Id == id)
        .SingleOrDefaultAsync();
    }

    public Task<Sales> GetSaleAsync(Expression<Func<Sales, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Sales>> GetSaleByCustomerIdAsync(string customerId)
    {
        return await _context.Sales
        .Include(c => c.Order)
        .Include(c => c.Order.Customer)
        .Where(c => c.Order.CustomerId == customerId)
        .ToListAsync();
    }

    public async Task<decimal> GetTotalMonthlySalesAsync()
    {
        return await _context.Sales
        .Where(x => x.CreatedAt.Month == DateTime.Now.Month)
        .SumAsync(x => x.AmountPaid);
    }
    public async Task<decimal> GetTotalYearlySalesAsync()
    {
        return await _context.Sales
        .Where(x => x.CreatedAt.Year == DateTime.Now.Year)
        .SumAsync(x => x.AmountPaid);
    }
    public async Task<decimal> GetTotalMonthlySalesAsync(int month, int year)
    {
        return await _context.Sales
        .Where(x => x.CreatedAt.Month == month && x.CreatedAt.Year == year)
        .SumAsync(x => x.AmountPaid);
    }
    public async Task<decimal> GetTotalYearlySalesAsync(int year)
    {
        return await _context.Sales
        .Where(x => x.CreatedAt.Year == year)
        .SumAsync(x => x.AmountPaid);
    }

}
