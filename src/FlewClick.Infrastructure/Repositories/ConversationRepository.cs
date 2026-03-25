using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class ConversationRepository(FlewClickDbContext context) : IConversationRepository
{
    public async Task<Conversation?> GetByBookingIdAsync(Guid bookingRequestId, CancellationToken ct = default) =>
        await context.Conversations.FirstOrDefaultAsync(c => c.BookingRequestId == bookingRequestId, ct);

    public async Task<Conversation?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Conversations.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task AddAsync(Conversation conversation, CancellationToken ct = default)
    {
        await context.Conversations.AddAsync(conversation, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Conversation conversation, CancellationToken ct = default)
    {
        context.Conversations.Update(conversation);
        await context.SaveChangesAsync(ct);
    }
}
