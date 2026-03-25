using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IAgreementRepository
{
    Task<Agreement?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Agreement>> GetByBookingIdAsync(Guid bookingRequestId, CancellationToken ct = default);
    Task<Agreement?> GetLatestByBookingIdAsync(Guid bookingRequestId, CancellationToken ct = default);
    Task<int> GetNextVersionAsync(Guid bookingRequestId, CancellationToken ct = default);
    Task AddAsync(Agreement agreement, CancellationToken ct = default);
    Task UpdateAsync(Agreement agreement, CancellationToken ct = default);
}
