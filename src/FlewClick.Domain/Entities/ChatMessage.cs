using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class ChatMessage : Entity
{
    public Guid ConversationId { get; private init; }
    public Guid SenderId { get; private init; }
    public MessageSenderType SenderType { get; private init; }
    public string Content { get; private init; } = string.Empty;
    public bool IsRead { get; private set; }
    public DateTime? ReadAt { get; private set; }

    private ChatMessage() { }

    public static ChatMessage Create(Guid conversationId, Guid senderId, MessageSenderType senderType, string content)
    {
        if (conversationId == Guid.Empty) throw new DomainException("Conversation ID is required.");
        if (senderId == Guid.Empty) throw new DomainException("Sender ID is required.");
        if (string.IsNullOrWhiteSpace(content)) throw new DomainException("Message content is required.");

        return new ChatMessage
        {
            ConversationId = conversationId,
            SenderId = senderId,
            SenderType = senderType,
            Content = content.Trim(),
            IsRead = false
        };
    }

    public void MarkAsRead()
    {
        if (IsRead) return;
        IsRead = true;
        ReadAt = DateTime.UtcNow;
        Touch();
    }
}
