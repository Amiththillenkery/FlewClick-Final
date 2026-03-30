using FlewClick.Application.Features.Chat.Common;
using FlewClick.Domain.Enums;

namespace FlewClick.Application.Interfaces;

public interface INotificationService
{
    Task NotifyBookingUpdatedAsync(Guid bookingId, Guid consumerId, Guid professionalId, BookingStatus status, string message, CancellationToken ct = default);
    Task NotifyNewBookingRequestAsync(Guid professionalId, Guid bookingId, string consumerFullName, CancellationToken ct = default);
    Task NotifyNewMessageAsync(Guid consumerId, Guid professionalId, ChatMessageDto message, CancellationToken ct = default);
}
