using FlewClick.Domain.Common;
using FlewClick.Domain.Enums;
using FlewClick.Domain.Exceptions;

namespace FlewClick.Domain.Entities;

public class BookingStatusHistory : Entity
{
    public Guid BookingRequestId { get; private init; }
    public BookingStatus? FromStatus { get; private init; }
    public BookingStatus ToStatus { get; private init; }
    public string ChangedBy { get; private init; } = string.Empty;
    public MessageSenderType ChangedByType { get; private init; }
    public string? Reason { get; private init; }

    private BookingStatusHistory() { }

    public static BookingStatusHistory Create(
        Guid bookingRequestId, BookingStatus? fromStatus, BookingStatus toStatus,
        string changedBy, MessageSenderType changedByType, string? reason = null)
    {
        if (bookingRequestId == Guid.Empty) throw new DomainException("Booking request ID is required.");
        if (string.IsNullOrWhiteSpace(changedBy)) throw new DomainException("ChangedBy is required.");

        return new BookingStatusHistory
        {
            BookingRequestId = bookingRequestId,
            FromStatus = fromStatus,
            ToStatus = toStatus,
            ChangedBy = changedBy,
            ChangedByType = changedByType,
            Reason = reason?.Trim()
        };
    }
}
