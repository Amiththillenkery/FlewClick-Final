using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Chat.MarkMessagesRead;

public record MarkMessagesReadCommand(Guid BookingRequestId, Guid RecipientId) : IRequest;

public class MarkMessagesReadValidator : AbstractValidator<MarkMessagesReadCommand>
{
    public MarkMessagesReadValidator()
    {
        RuleFor(x => x.BookingRequestId).NotEmpty();
        RuleFor(x => x.RecipientId).NotEmpty();
    }
}

public class MarkMessagesReadHandler(
    IConversationRepository conversationRepository,
    IChatMessageRepository chatMessageRepository)
    : IRequestHandler<MarkMessagesReadCommand>
{
    public async Task Handle(MarkMessagesReadCommand request, CancellationToken ct)
    {
        var conversation = await conversationRepository.GetByBookingIdAsync(request.BookingRequestId, ct)
            ?? throw new EntityNotFoundException("Conversation", request.BookingRequestId);

        if (request.RecipientId != conversation.ConsumerId
            && request.RecipientId != conversation.ProfessionalProfileId)
            throw new DomainException("Recipient is not a participant in this conversation.");

        await chatMessageRepository.MarkAsReadAsync(conversation.Id, request.RecipientId, ct);
    }
}
