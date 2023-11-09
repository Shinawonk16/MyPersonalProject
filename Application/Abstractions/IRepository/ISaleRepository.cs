using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;

public interface ISaleRepository : IBaseRepository<Sales>
{
    Task<Sales> GetSaleAsync(string id);
    Task<IList<Sales>> GetSaleByCustomerIdAsync(string customerId);
    Task<IList<Sales>> GetAllSalesOfTheYearAsync();
    Task<IList<Sales>> GetAllSalesOfTheMonthAsync();
    Task<IList<Sales>> GetAllSaleAsync();
    Task<Sales> GetSaleAsync(Expression<Func<Sales, bool>> expression);
    Task<IList<Sales>> GetAllSaleAsync(Expression<Func<Sales, bool>> expression);
    Task<decimal> GetTotalMonthlySalesAsync();
    Task<decimal> GetTotalYearlySalesAsync();
    Task<decimal> GetTotalMonthlySalesAsync(int month, int year);
    Task<decimal> GetTotalYearlySalesAsync(int year);
}