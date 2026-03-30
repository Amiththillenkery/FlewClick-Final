using FlewClick.Application.Features.Chat.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Entities;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Chat.SendMessage;

public record SendMessageCommand(
    Guid BookingRequestId,
    Guid SenderId,
    MessageSenderType SenderType,
    string Content) : IRequest<ChatMessageDto>;

public class SendMessageValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageValidator()
    {
        RuleFor(x => x.BookingRequestId).NotEmpty();
        RuleFor(x => x.SenderId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MaximumLength(8000);
    }
}

public class SendMessageHandler(
    IConversationRepository conversationRepository,
    IChatMessageRepository chatMessageRepository,
    INotificationService notificationService)
    : IRequestHandler<SendMessageCommand, ChatMessageDto>
{
    public async Task<ChatMessageDto> Handle(SendMessageCommand request, CancellationToken ct)
    {
        var conversation = await conversationRepository.GetByBookingIdAsync(request.BookingRequestId, ct)
            ?? throw new EntityNotFoundException("Conversation", request.BookingRequestId);

        if (!conversation.IsActive)
            throw new DomainException("This conversation is not active.");

        ValidateSender(conversation, request.SenderId, request.SenderType);

        var message = ChatMessage.Create(conversation.Id, request.SenderId, request.SenderType, request.Content);
        await chatMessageRepository.AddAsync(message, ct);

        var dto = ChatMapper.ToDto(message);

        await notificationService.NotifyNewMessageAsync(
            conversation.ConsumerId,
            conversation.ProfessionalProfileId,
            dto,
            ct);

        return dto;
    }

    private static void ValidateSender(Conversation conversation, Guid senderId, MessageSenderType senderType)
    {
        switch (senderType)
        {
            case MessageSenderType.Consumer:
                if (senderId != conversation.ConsumerId)
                    throw new DomainException("Sender does not match the consumer for this conversation.");
                break;
            case MessageSenderType.Professional:
                if (senderId != conversation.ProfessionalProfileId)
                    throw new DomainException("Sender does not match the professional for this conversation.");
                break;
            case MessageSenderType.System:
                throw new DomainException("System messages cannot be sent through this command.");
            default:
                throw new DomainException("Unsupported sender type.");
        }
    }
}
