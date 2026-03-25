using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IConversationRepository
{
    Task<Conversation?> GetByBookingIdAsync(Guid bookingRequestId, CancellationToken ct = default);
    Task<Conversation?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Conversation conversation, CancellationToken ct = default);
    Task UpdateAsync(Conversation conversation, CancellationToken ct = default);
}
