using FlewClick.Application.Features.Chat.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Application.Features.Chat.GetInbox;

public record InboxItemDto(
    Guid ConversationId,
    Guid BookingRequestId,
    string OtherPartyName,
    string? LastMessage,
    DateTime? LastMessageAt,
    int UnreadCount,
    BookingStatus BookingStatus);

public record GetInboxQuery(Guid UserId) : IRequest<List<InboxItemDto>>;

public class GetInboxHandler(
    IConversationRepository conversationRepository,
    IChatMessageRepository messageRepository,
    IBookingRequestRepository bookingRepository,
    IConsumerRepository consumerRepository,
    IProfessionalProfileRepository professionalProfileRepository,
    IAppUserRepository userRepository)
    : IRequestHandler<GetInboxQuery, List<InboxItemDto>>
{
    public async Task<List<InboxItemDto>> Handle(GetInboxQuery request, CancellationToken ct)
    {
        var conversations = await conversationRepository.GetByUserIdAsync(request.UserId, ct);
        var result = new List<InboxItemDto>();

        foreach (var conv in conversations)
        {
            var booking = await bookingRepository.GetByIdAsync(conv.BookingRequestId, ct);
            if (booking == null) continue;

            string otherPartyName = "Unknown";
            if (conv.ConsumerId == request.UserId)
            {
                // Current user is consumer, get professional name
                var prof = await professionalProfileRepository.GetByIdAsync(conv.ProfessionalProfileId, ct);
                if (prof != null)
                {
                    var user = await userRepository.GetByIdAsync(prof.AppUserId, ct);
                    otherPartyName = user?.FullName ?? "Professional";
                }
            }
            else
            {
                // Current user is professional, get consumer name
                var consumer = await consumerRepository.GetByIdAsync(conv.ConsumerId, ct);
                if (consumer != null)
                {
                    otherPartyName = consumer.FullName;
                }
            }

            var lastMsg = await messageRepository.GetLastMessageAsync(conv.Id, ct);
            var unreadCount = await messageRepository.GetUnreadCountAsync(conv.Id, request.UserId, ct);

            result.Add(new InboxItemDto(
                conv.Id,
                conv.BookingRequestId,
                otherPartyName,
                lastMsg?.Content,
                lastMsg?.CreatedAtUtc,
                unreadCount,
                booking.Status));
        }

        return result.OrderByDescending(x => x.LastMessageAt ?? DateTime.MinValue).ToList();
    }
}
