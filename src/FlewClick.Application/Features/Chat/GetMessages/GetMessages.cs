using FlewClick.Application.Features.Chat.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace FlewClick.Application.Features.Chat.GetMessages;

public record GetMessagesQuery(Guid BookingRequestId, int Page, int PageSize) : IRequest<List<ChatMessageDto>>;

public class GetMessagesValidator : AbstractValidator<GetMessagesQuery>
{
    public GetMessagesValidator()
    {
        RuleFor(x => x.BookingRequestId).NotEmpty();
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}

public class GetMessagesHandler(
    IConversationRepository conversationRepository,
    IChatMessageRepository chatMessageRepository)
    : IRequestHandler<GetMessagesQuery, List<ChatMessageDto>>
{
    public async Task<List<ChatMessageDto>> Handle(GetMessagesQuery request, CancellationToken ct)
    {
        var conversation = await conversationRepository.GetByBookingIdAsync(request.BookingRequestId, ct)
            ?? throw new EntityNotFoundException("Conversation", request.BookingRequestId);

        var messages = await chatMessageRepository.GetByConversationIdAsync(
            conversation.Id, request.Page, request.PageSize, ct);

        return messages.Select(ChatMapper.ToDto).ToList();
    }
}
