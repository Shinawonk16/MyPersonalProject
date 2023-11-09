using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;

public interface IUserRepository:IBaseRepository<User>
{
    Task<UserRole> LoginAsync(string email,string password);
    Task<UserRole> GetAsync(Expression<Func<UserRole,bool>> expression);

    Task<IList<UserRole>> GetUserByRoleAsync(string role);
}
