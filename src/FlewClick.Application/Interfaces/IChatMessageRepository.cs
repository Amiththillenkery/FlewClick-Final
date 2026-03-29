using FlewClick.Domain.Entities;

namespace FlewClick.Application.Interfaces;

public interface IChatMessageRepository
{
    Task<List<ChatMessage>> GetByConversationIdAsync(Guid conversationId, int page = 1, int pageSize = 50, CancellationToken ct = default);
    Task<int> GetUnreadCountAsync(Guid conversationId, Guid recipientId, CancellationToken ct = default);
    Task<ChatMessage?> GetLastMessageAsync(Guid conversationId, CancellationToken ct = default);
    Task AddAsync(ChatMessage message, CancellationToken ct = default);
    Task MarkAsReadAsync(Guid conversationId, Guid recipientId, CancellationToken ct = default);
}
