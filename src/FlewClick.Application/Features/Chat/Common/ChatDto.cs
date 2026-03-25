using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.Chat.Common;

public record ConversationDto(
    Guid Id,
    Guid BookingRequestId,
    Guid ConsumerId,
    Guid ProfessionalProfileId,
    bool IsActive,
    int UnreadCount,
    DateTime CreatedAtUtc);

public record ChatMessageDto(
    Guid Id,
    Guid ConversationId,
    Guid SenderId,
    MessageSenderType SenderType,
    string Content,
    bool IsRead,
    DateTime? ReadAt,
    DateTime CreatedAtUtc);

public static class ChatMapper
{
    public static ConversationDto ToConversationDto(Conversation c, int unreadCount) =>
        new(c.Id, c.BookingRequestId, c.ConsumerId, c.ProfessionalProfileId, c.IsActive, unreadCount, c.CreatedAtUtc);

    public static ChatMessageDto ToDto(ChatMessage m) =>
        new(m.Id, m.ConversationId, m.SenderId, m.SenderType,
            m.Content, m.IsRead, m.ReadAt, m.CreatedAtUtc);
}
