using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.IRepository;

public interface ILikeRepository : IBaseRepository<Like>
{
    Task<IEnumerable<Like>> GetLikesAsync(string reveiwId);
    Task<Like> GetLikeAsync(Expression<Func<Like, bool>> expression);


}