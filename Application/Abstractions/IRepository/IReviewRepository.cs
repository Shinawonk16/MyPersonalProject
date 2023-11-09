using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;

public interface IReviewRepository : IBaseRepository<Review>
{
    Task<IList<Review>> GetReviewByProductIdAsync(string productId);
    Task<Review> GetReviewAsync(Expression<Func<Review, bool>> expression);
    Task<IList<Review>> GetAllSelectedReviewAsync(Expression<Func<Review, bool>> expression);
    Task<IList<Review>> GetAllReviewAsync();
    Task<IList<Review>> GetAllReviewByCustomerIdAsync(string customerId);

}
