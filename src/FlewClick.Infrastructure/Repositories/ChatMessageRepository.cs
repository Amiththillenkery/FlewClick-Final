using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlewClick.Infrastructure.Repositories;

public class ChatMessageRepository(FlewClickDbContext context) : IChatMessageRepository
{
    public async Task<List<ChatMessage>> GetByConversationIdAsync(
        Guid conversationId, int page = 1, int pageSize = 50, CancellationToken ct = default)
    {
        var safePage = Math.Max(1, page);
        var skip = (safePage - 1) * pageSize;

        return await context.ChatMessages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.CreatedAtUtc)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> GetUnreadCountAsync(Guid conversationId, Guid recipientId, CancellationToken ct = default) =>
        await context.ChatMessages.CountAsync(
            m => m.ConversationId == conversationId && m.SenderId != recipientId && !m.IsRead,
            ct);

    public async Task AddAsync(ChatMessage message, CancellationToken ct = default)
    {
        await context.ChatMessages.AddAsync(message, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task MarkAsReadAsync(Guid conversationId, Guid recipientId, CancellationToken ct = default)
    {
        var unread = await context.ChatMessages
            .Where(m => m.ConversationId == conversationId && m.SenderId != recipientId && !m.IsRead)
            .ToListAsync(ct);

        foreach (var message in unread)
            message.MarkAsRead();

        if (unread.Count > 0)
            await context.SaveChangesAsync(ct);
    }
}
