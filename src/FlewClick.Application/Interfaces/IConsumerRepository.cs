using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IConsumerRepository
{
    Task<Consumer?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Consumer?> GetByPhoneAsync(string phone, CancellationToken ct = default);
    Task AddAsync(Consumer consumer, CancellationToken ct = default);
    Task UpdateAsync(Consumer consumer, CancellationToken ct = default);
}
