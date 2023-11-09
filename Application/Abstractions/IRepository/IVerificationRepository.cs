using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;

public interface IVerificationRepository : IBaseRepository<Verification>
{
   Task<Verification> GetAsync(string id);
    Task<Verification> GetAsync(Expression<Func<Verification, bool>> expression);
}
