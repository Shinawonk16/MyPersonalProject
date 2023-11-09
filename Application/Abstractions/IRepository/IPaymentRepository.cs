using Domain.Entities;

namespace Application.Abstractions.IRepository;
public interface IPaymentRepository : IBaseRepository<Payment>
{
    Task<Payment> GetPaymentAsync(string referenceNumber);
    Task<IEnumerable<Payment>> GetAllPaymentAsync();
}