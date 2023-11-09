using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;

public interface IRequestRepository: IBaseRepository<Request>
{
    Task<Request> GetRequestAsync(string id);
    Task<Request> GetSelectedRequestAsync(Expression<Func<Request, bool>> expression);
    Task<decimal> GetSumOfApprovedRequestOfTheMonthAsync(int month, int year);
    Task<decimal> GetSumOfApprovedReqeustForTheMonthAsync();
    Task<decimal> GetSumOfApprovedRequestForTheYearAsync(int year);
    Task<decimal> GetSumOfApprovedRequestForTheYearAsync();
    Task<IEnumerable<Request>> GetAllRequestAsync();
    Task<IList<Request>> GetAllSelected(Expression<Func<Request, bool>> expression);
}
