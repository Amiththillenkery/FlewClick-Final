using FlewClick.Application.Features.Chat.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Chat.GetConversation;

public record GetConversationQuery(Guid BookingRequestId, Guid RequesterId) : IRequest<ConversationDto>;

public class GetConversationValidator : AbstractValidator<GetConversationQuery>
{
    public GetConversationValidator()
    {
        RuleFor(x => x.BookingRequestId).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
    }
}

public class GetConversationHandler(
    IConversationRepository conversationRepository,
    IChatMessageRepository chatMessageRepository)
    : IRequestHandler<GetConversationQuery, ConversationDto>
{
    public async Task<ConversationDto> Handle(GetConversationQuery request, CancellationToken ct)
    {
        var conversation = await conversationRepository.GetByBookingIdAsync(request.BookingRequestId, ct)
            ?? throw new EntityNotFoundException("Conversation", request.BookingRequestId);

        if (request.RequesterId != conversation.ConsumerId
            && request.RequesterId != conversation.ProfessionalProfileId)
            throw new DomainException("You do not have access to this conversation.");

        var unread = await chatMessageRepository.GetUnreadCountAsync(conversation.Id, request.RequesterId, ct);
        return ChatMapper.ToConversationDto(conversation, unread);
    }
}
