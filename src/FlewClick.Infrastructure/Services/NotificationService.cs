using FlewClick.Application.Features.Chat.Common;
using FlewClick.Application.Interfaces;
using FlewClick.Domain.Enums;
using FlewClick.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FlewClick.Infrastructure.Services;

public class NotificationService(IHubContext<AppHub> hubContext) : INotificationService
{
    public async Task NotifyBookingUpdatedAsync(Guid bookingId, Guid consumerId, Guid professionalId, BookingStatus status, string message, CancellationToken ct = default)
    {
        var update = new {
            BookingId = bookingId,
            Status = status,
            Message = message
        };

        // Notify the specific project room (for anyone viewing the detail page)
        await hubContext.Clients.Group(bookingId.ToString()).SendAsync("BookingUpdated", update, ct);

        // Notify the consumer's personal room (for history list updates)
        await hubContext.Clients.Group(consumerId.ToString()).SendAsync("BookingUpdated", update, ct);

        // Notify the professional's personal room (for history/incoming list updates)
        await hubContext.Clients.Group(professionalId.ToString()).SendAsync("BookingUpdated", update, ct);
    }

    public async Task NotifyNewBookingRequestAsync(Guid professionalId, Guid bookingId, string consumerFullName, CancellationToken ct = default)
    {
        await hubContext.Clients.Group(professionalId.ToString()).SendAsync("NewBookingRequest", new {
            BookingId = bookingId,
            ConsumerName = consumerFullName,
            Message = $"New booking request from {consumerFullName}"
        }, ct);
    }

    public async Task NotifyNewMessageAsync(Guid consumerId, Guid professionalId, ChatMessageDto message, CancellationToken ct = default)
    {
        // Broadcast to personal rooms of both participants. 
        // This ensures Inbox refreshes and messages appear in active chats 
        // without double-delivery when in both rooms.
        await hubContext.Clients.Group(consumerId.ToString()).SendAsync("ReceiveMessage", message, ct);
        await hubContext.Clients.Group(professionalId.ToString()).SendAsync("ReceiveMessage", message, ct);
    }
}
