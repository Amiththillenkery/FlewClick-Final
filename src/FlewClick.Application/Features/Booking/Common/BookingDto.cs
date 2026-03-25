using FlewClick.Domain.Enums;

namespace FlewClick.Application.Features.Booking.Common;

public record BookingDto(
    Guid Id,
    Guid ConsumerId,
    Guid ProfessionalProfileId,
    Guid PackageId,
    DateTime EventDate,
    string? EventLocation,
    string? Notes,
    BookingStatus Status,
    string? DeclineReason,
    string? CancellationReason,
    MessageSenderType? CancelledBy,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);

public record BookingDetailDto(
    BookingDto Booking,
    string? ConsumerName,
    string? ProfessionalName,
    string? PackageName
);

public static class BookingMapper
{
    public static BookingDto ToDto(FlewClick.Domain.Entities.BookingRequest b) =>
        new(b.Id, b.ConsumerId, b.ProfessionalProfileId, b.PackageId,
            b.EventDate, b.EventLocation, b.Notes, b.Status,
            b.DeclineReason, b.CancellationReason, b.CancelledBy,
            b.CreatedAtUtc, b.UpdatedAtUtc);
}
